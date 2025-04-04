import { Octokit } from '@octokit/rest';
import { openDB, DBSchema, IDBPDatabase } from 'idb';
import { Recipe } from '../types/recipe';
import yaml from 'js-yaml';

interface RecipeDb extends DBSchema {
    recipes: {
        key: string;
        value: {
            id: string;
            content: Recipe;
        };
    };
}

class GitHubService {
    private octokit: Octokit;
    private dbPromise: Promise<IDBPDatabase<RecipeDb>>;

    constructor() {
        this.octokit = new Octokit();
        this.dbPromise = openDB<RecipeDb>('recipes', 1, {
            upgrade(db) {
                db.createObjectStore('recipes', { keyPath: 'id' });
            },
        });
    }

    public async downloadAndStoreFiles(owner: string, repo: string): Promise<Recipe[]> {
        const files = await this.getAllFiles(owner, repo);
        const db = await this.dbPromise;

        const recipes: Recipe[] = [];

        for (const file of files) {
            const content = await this.getFileContent(owner, repo, file.path);
            const rawRecipe = yaml.load(content) as any;

            rawRecipe.prepTime = rawRecipe['prep-time'];
            rawRecipe.cookTime = rawRecipe['cook-time'];
            const recipe: Recipe = {
                ...rawRecipe,
                ingredients: rawRecipe.ingredients.map((ingredient: any) => {
                    const [key, value] = Object.entries(ingredient)[0] as [string, string];
                    const [quantity, ...unitParts] = value.toString().split(' ');
                    return {
                        name: key,
                        quantity: parseFloat(quantity),
                        unit: unitParts.join(' '),
                        ...ingredient
                    };
                })
            };
            recipes.push(recipe);
            await db.put('recipes', { id: file.path, content: recipe});
        }

        return recipes;
    }

    private async getAllFiles(owner: string, repo: string, path = ''): Promise<{ path: string }[]> {
        const response = await this.octokit.repos.getContent({ owner, repo, path });
        const files: { path: string }[] = [];

        if (Array.isArray(response.data)) {
            for (const item of response.data) {
                if (item.type === 'file') {
                    files.push({ path: item.path });
                } else if (item.type === 'dir') {
                    const subFiles = await this.getAllFiles(owner, repo, item.path);
                    files.push(...subFiles);
                }
            }
        }

        return files;
    }

    private async getFileContent(owner: string, repo: string, path: string): Promise<string> {
        const response = await this.octokit.repos.getContent({ owner, repo, path });
        if ('content' in response.data) {
            return atob(response.data.content);
        }
        throw new Error('File content not found');
    }
}

export default GitHubService;

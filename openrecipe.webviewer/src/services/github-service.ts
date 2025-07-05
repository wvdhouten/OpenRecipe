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
    tags: {
        key: string;
        value: {
            id: string;
            content: string;
        };
    };
    assets: {
        key: string;
        value: {
            id: string;
            content: string;
        };
    };
}

class GitHubService {
    private octokit: Octokit;
    private dbPromise: Promise<IDBPDatabase<RecipeDb>>;

    constructor() {
        let token = localStorage.getItem('githubToken');
        const octoParams = token !== null && token !== "" ? { auth: token } : {};
        this.octokit = new Octokit(octoParams);
        this.dbPromise = openDB<RecipeDb>('recipes', 1.1, {
            upgrade(db) {
                if (!db.objectStoreNames.contains('recipes')) {
                    db.createObjectStore('recipes', { keyPath: 'id', autoIncrement: true });
                }
                if (!db.objectStoreNames.contains('tags')) {
                    db.createObjectStore('tags', { keyPath: 'id' });
                }
                if (!db.objectStoreNames.contains('assets')) {
                    db.createObjectStore('assets', { keyPath: 'id' });
                }
            },
        });
    }

    public async downloadAndStoreRecipes(owner: string, repo: string): Promise<Recipe[]> {
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
            await db.put('recipes', { id: file.path, content: recipe });
        }

        return recipes;
    }

    public async downloadAndStoreAssets(owner: string, repo: string): Promise<void> {
        const files = await this.getAllFiles(owner, repo, 'assets');
        const db = await this.dbPromise;

        for (const file of files) {
            const content = await this.getFileContent(owner, repo, file.path);
            await db.put('assets', { id: file.path, content: content });
        }
    }

    private async getAllFiles(owner: string, repo: string, path = '', recursive = false): Promise<{ path: string }[]> {
        const response = await this.octokit.repos.getContent({ owner, repo, path });
        const files: { path: string }[] = [];

        if (!Array.isArray(response.data)) {
            return files;
        }

        for (const item of response.data) {
            if (item.type === 'file') {
                files.push({ path: item.path });
            } else if (item.type === 'dir' && recursive) {
                const subFiles = await this.getAllFiles(owner, repo, item.path);
                files.push(...subFiles);
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

    private async writeFileContent(owner: string, repo: string, path: string, content: string, message: string = 'Update file'): Promise<void> {
        // Get the current file SHA if it exists (required for updates)
        let sha: string | undefined = undefined;
        try {
            const response = await this.octokit.repos.getContent({ owner, repo, path });
            if ('sha' in response.data) {
                sha = response.data.sha;
            }
        } catch (error: any) {
            // If file does not exist, ignore error (will create new file)
            if (error.status !== 404) {
                throw error;
            }
        }

        await this.octokit.repos.createOrUpdateFileContents({ owner, repo, path, message, content: btoa(content), sha });
    }
}

export default GitHubService;

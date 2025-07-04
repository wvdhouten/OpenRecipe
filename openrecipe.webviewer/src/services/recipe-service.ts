import { openDB, IDBPDatabase } from 'idb';
import { Recipe } from '../types/recipe';
import GithubService from './github-service';

class RecipeService {
    private dbPromise: Promise<IDBPDatabase>;
    private tagsStoreName = 'tags'; // Store name for tags

    constructor() {
        this.dbPromise = openDB('recipes', 1.1, {
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

    async addRecipe(recipe: Omit<Recipe, 'id'>): Promise<string> {
        const db = await this.dbPromise;
        return db.add('recipes', recipe).then((id) => id.toString());
    }

    async getRecipe(id: string): Promise<Recipe | null> {
        const db = await this.dbPromise;
        return db.get('recipes', id);
    }

    async getAsset(id: string): Promise<string> {
        const db = await this.dbPromise;
        return db.get('assets', id);
    }

    async getAllRecipes(): Promise<Recipe[]> {
        const db = await this.dbPromise;
        return db.getAll('recipes');
    }

    async updateRecipe(recipe: Recipe): Promise<void> {
        const db = await this.dbPromise;
        await db.put('recipes', recipe);
    }

    async deleteRecipe(id: string): Promise<void> {
        const db = await this.dbPromise;
        await db.delete('recipes', id);
    }

    async ensureRecipes(): Promise<void> {
    }

    async refresh(): Promise<void> {
        const githubService = new GithubService();
        const recipes = await githubService.downloadAndStoreRecipes("wvdhouten", "recipes");
        githubService.downloadAndStoreAssets("wvdhouten", "recipes");
        const db = await this.dbPromise;

        await db.clear('recipes');
        await db.clear(this.tagsStoreName); // Clear tags store before refreshing

        const tx = db.transaction(['recipes', 'tags'], 'readwrite');
        const tagsSet = new Set<string>(); // Temporary in-memory set for tags

        for (const recipe of recipes) {
            await tx.objectStore('recipes').put(recipe);

            if (recipe.tags) {
                recipe.tags.forEach(tag => tagsSet.add(tag));
            }
        }

        const tagsArray = Array.from(tagsSet);
        for (const tag of tagsArray)
            await tx.objectStore(this.tagsStoreName).put({ id: tag });

        await tx.done;
    }

    async getKnownTags(): Promise<string[]> {
        const db = await this.dbPromise;
        const tags = await db.getAll(this.tagsStoreName);
        return tags.map(tagEntry => tagEntry.id);
    }
}

export default RecipeService;
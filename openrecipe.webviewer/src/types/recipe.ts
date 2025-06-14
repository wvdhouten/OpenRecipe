import { Ingredient } from "./ingredient";

export interface Recipe {
    id?: string;
    name: string;
    description?: string;
    reference?: string;
    servings?: number;
    prepTime?: number;
    cookTime?: number;
    ingredients: Ingredient[];
    instructions: string[];
    nutrition?: { [key: string]: number };
    notes: string[];
    tags: string[];
    metadata?: { [key: string]: string };
}
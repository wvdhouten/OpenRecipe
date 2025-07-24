import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ulid } from 'ulid';
import { Recipe } from '../types/recipe';
//import { Ingredient } from '../types/ingredient';
import RecipeService from '../services/recipe-service';

const recipeService = new RecipeService();

const Editor: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [recipe, setRecipe] = useState<Recipe | null>(null);
    const [ingredientInput, setIngredientInput] = useState<string>('');

    useEffect(() => {
        loadRecipe(id ?? "", true);
    }, []);

    function loadRecipe(id: string, retry: boolean) {
        if (!id) {
            setRecipe({ id: ulid(), name: "", ingredients: [], instructions: [], notes: [], tags: [] })
            return;
        }

        recipeService.getRecipe(id ?? "").then(data => {
            if (!data) {
                if (retry)
                    recipeService.refresh().then(() => { loadRecipe(id, false); })
                else
                    navigate('/');
            }

            setRecipe(data);
        });
    }

    const handleIngredientChange = (index: number, value: string) => {
        if (!recipe) return;
        const newIngredients = [...(recipe.ingredients || [])];
        newIngredients[index] = { ...newIngredients[index], name: value };
        setRecipe({ ...recipe, ingredients: newIngredients });
    };

    const handleIngredientAdd = () => {
        if (!recipe || !ingredientInput.trim()) return;
        //const newIngredients = [...(recipe.ingredients || []), { name: ingredientInput.trim() }];
        //setRecipe({ ...recipe, ingredients: newIngredients });
        setIngredientInput('');
    };

    const handleIngredientRemove = (index: number) => {
        if (!recipe) return;
        const newIngredients = [...(recipe.ingredients || [])];
        newIngredients.splice(index, 1);
        setRecipe({ ...recipe, ingredients: newIngredients });
    };

    const handleSave = async () => {
        if (!recipe) return;
        await recipeService.updateRecipe(recipe);
        setRecipe(null);
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement> | React.ChangeEvent<HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setRecipe({ ...recipe, [name]: value } as Recipe);
    };

    return (
        <div className="container py-4">
            {recipe && (
                <div className="card shadow-sm mx-auto" style={{ maxWidth: 600 }}>
                    <div className="card-body">
                        <h2 className="card-title mb-4">Edit Recipe</h2>
                        <form>
                            <div className="mb-3">
                                <label htmlFor="recipeName" className="form-label">Recipe Name</label>
                                <input type="text" className="form-control" id="recipeName" name="name" placeholder="Recipe Name" value={recipe.name} onChange={handleChange} />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="recipeDescription" className="form-label">Description</label>
                                <textarea className="form-control" id="recipeDescription" name="description" placeholder="Description" value={recipe.description || ""} onChange={handleChange} rows={3} />
                            </div>
                            <div className="mb-3">
                                <label className="form-label">Ingredients</label>
                                <ul className="list-group mb-2">
                                    {(recipe.ingredients || []).map((ingredient, idx) => (
                                        <li key={idx} className="list-group-item d-flex align-items-center gap-2">
                                            <input type="text" className="form-control" value={ingredient.name} onChange={e => handleIngredientChange(idx, e.target.value)} />
                                            <button type="button" className="btn btn-outline-danger btn-sm" onClick={() => handleIngredientRemove(idx)} title="Remove">
                                                <i className="bi bi-x"></i> Remove
                                            </button>
                                        </li>
                                    ))}
                                </ul>
                                <div className="input-group">
                                    <input type="text" className="form-control" value={ingredientInput} onChange={e => setIngredientInput(e.target.value)} placeholder="Add ingredient" />
                                    <button type="button" className="btn btn-outline-primary" onClick={handleIngredientAdd}>Add Ingredient</button>
                                </div>
                            </div>
                            <div className="d-flex justify-content-end gap-2 mt-4">
                                <button type="button" className="btn btn-primary" onClick={handleSave}>Save</button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Editor;
import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import RecipeService from '../services/recipe-service';
import { Recipe } from '../types/recipe';
import { Ingredient } from '../types/ingredient';

const RecipeDetails: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [recipe, setRecipe] = useState<Recipe | null>(null);
    const [desiredServings, setDesiredServings] = useState<number>(1);
    const recipeService = new RecipeService();

    useEffect(() => {
        loadRecipe(id ?? "", true);
    }, [id]);


    function loadRecipe(id: string, retry: boolean) {
        recipeService.getRecipe(id ?? "").then(data =>
        {
            if (!data) {
                if (retry)
                    recipeService.refresh().then(() => { loadRecipe(id, false); })
                else
                    navigate('/recipes');
            }

            setRecipe(data); setDesiredServings(data?.servings ?? 1);
        });
    }

    function scaleIngredient(ingredient: Ingredient): number {
        const scaledQuantity = ingredient.quantity * (desiredServings / (recipe?.servings ?? 1));
        return +scaledQuantity.toFixed(2);
    };

    if (!recipe) {
        return <p>Loading...</p>;
    }

    return (
        <article className="container mt-4">
            <header className="bg-dark text-light rounded">
                <div className="d-flex justify-content-between align-items-center">
                    <h1 className="display-5">{recipe.name}</h1>
                    <button className="btn btn-outline-secondary" onClick={() => navigate('/recipes')} title="Refresh Recipes">
                        <i className="bi bi-arrow-left"></i>
                    </button>
                </div>
                <p className="lead">{recipe.description}</p>
                <div className="mb-3">
                    {recipe.tags.map((tag, index) => (
                        <span key={index} className="badge bg-warning text-dark me-2">{tag}</span>
                    ))}
                </div>
                <hr />
                <div className="d-flex justify-content-between">
                    <div>
                        <strong>Prep Time:</strong> {recipe.prepTime} min
                    </div>
                    <div>
                        <strong>Cook Time:</strong> {recipe.cookTime} min
                    </div>
                    <div className="d-flex align-items-center">
                        <strong>Servings:</strong>
                        <button className="btn btn-link p-0 ms-2 me-2" onClick={() => setDesiredServings(desiredServings - 1)}>
                            <i className="bi bi-dash-circle"></i>
                        </button>
                        <span className="fw-bold">{desiredServings}</span>
                        <button className="btn btn-link p-0 ms-2 me-2" onClick={() => setDesiredServings(desiredServings + 1)}>
                            <i className="bi bi-plus-circle"></i>
                        </button>
                        <button className="btn btn-link p-0 ms-2" onClick={() => setDesiredServings(recipe.servings ?? 1)}>
                            <i className="bi bi-arrow-counterclockwise"></i>
                        </button>
                    </div>
                </div>
            </header>

            <section className="mt-4">
                <ul className="nav nav-tabs" id="tab-list" role="tablist">
                    <li className="nav-item" role="presentation">
                        <button className="nav-link active" id="ingredients-tab" data-bs-toggle="tab" data-bs-target="#ingredients-tab-pane" type="button" role="tab" aria-controls="ingredients-tab-pane" aria-selected="true">Ingredients</button>
                    </li>
                    <li className="nav-item" role="presentation">
                        <button className="nav-link" id="instructions-tab" data-bs-toggle="tab" data-bs-target="#instructions-tab-pane" type="button" role="tab" aria-controls="instructions-tab-pane" aria-selected="false">Instructions</button>
                    </li>
                    <li className="nav-item" role="presentation" hidden={recipe.notes?.length < 1}>
                        <button className="nav-link" id="notes-tab" data-bs-toggle="tab" data-bs-target="#notes-tab-pane" type="button" role="tab" aria-controls="notes-tab-pane" aria-selected="false">Notes</button>
                    </li>
                </ul>
                <div className="tab-content mt-3" id="tab-content">
                    <div className="tab-pane show active" id="ingredients-tab-pane" role="tabpanel" aria-labelledby="ingredients-tab">
                        <ul className="list-group">
                            {recipe.ingredients.map((ingredient, index) => (
                                <li key={index} className="list-group-item">
                                    <label style={{ display: 'block' }}>
                                        <div className="align-items-center">
                                            <input className="form-check-input me-2" type="checkbox" />
                                            {ingredient.name}: {scaleIngredient(ingredient)} {ingredient.unit}
                                        </div>
                                        {ingredient.notes?.length > 0 && (
                                            <ul className="mt-2 ps-4">
                                                {ingredient.notes.map((note, noteIndex) => (
                                                    <li key={noteIndex} className="text-muted">{note}</li>
                                                ))}
                                            </ul>
                                        )}
                                    </label>
                                </li>
                            ))}
                        </ul>
                    </div>
                    <div className="tab-pane" id="instructions-tab-pane" role="tabpanel" aria-labelledby="instructions-tab">
                        <ul className="list-group">
                            {recipe.instructions.map((instruction, index) => (
                                <li key={index} className="list-group-item">
                                    <label>
                                        <input className="form-check-input me-2" type="checkbox" />
                                        {instruction}
                                    </label>
                                </li>
                            ))}
                        </ul>
                    </div>
                    <div className="tab-pane" id="notes-tab-pane" role="tabpanel" aria-labelledby="notes-tab" hidden={recipe.notes?.length < 1}>
                        <ul className="list-group">
                            {recipe.notes?.map((note, index) => (
                                <li key={index} className="list-group-item">{note}</li>
                            ))}
                        </ul>
                    </div>
                </div>
            </section>
        </article>
    );
};

export default RecipeDetails;

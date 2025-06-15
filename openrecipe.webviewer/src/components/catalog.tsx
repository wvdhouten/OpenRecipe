import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Recipe } from '../types/recipe';
import RecipeService from '../services/recipe-service';

const recipeService = new RecipeService();

const RecipeCatalog: React.FC = () => {
  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [filterText, setFilterText] = useState<string>('');
  const [tags, setTags] = useState<string[]>([]);
  const [selectedTags, setSelectedTags] = useState<Set<string>>(new Set());
  const navigate = useNavigate();

  useEffect(() => {
    refreshRecipes(false);
  }, []);

  const refreshRecipes = async (hard: boolean) => {
    if (hard) await recipeService.refresh();

    let recipes = await recipeService.getAllRecipes();
    if (!recipes.length) {
      await recipeService.refresh();
      recipes = await recipeService.getAllRecipes();
    }

    setRecipes(recipes);
    setTags(await recipeService.getKnownTags());
  };

  const toggleTag = (tag: string) => {
    setSelectedTags(prev => {
      const updated = new Set(prev);
      if (updated.has(tag)) {
        updated.delete(tag);
      } else {
        updated.add(tag);
      }
      return updated;
    });
  };

  const filteredRecipes = recipes.filter(recipe => {
    const matchesText = !filterText || recipe.name.toLowerCase().includes(filterText.toLowerCase());
    const matchesIngredient = !filterText || recipe.ingredients.some(ingredient => ingredient.name.toLowerCase().includes(filterText.toLowerCase()));
    const matchesTags =
      selectedTags.size === 0 || Array.from(selectedTags).every(tag => recipe.tags.includes(tag));
    return matchesText && matchesIngredient && matchesTags;
  });

  return (
    <div className="container mt-4">
      <header className="d-flex justify-content-between align-items-center">
        <h1 className="display-5 text-center">Recipes</h1>
        <button
          className="btn btn-outline-secondary"
          onClick={() => refreshRecipes(true)}
          title="Refresh Recipes"
        >
          <i className="bi bi-arrow-counterclockwise"></i>
        </button>
      </header>

      <div className="mb-4 d-flex justify-content-between align-items-center">
        <div className="input-group">
          <input
            type="text"
            className="form-control"
            placeholder="Filter by name or ingredient..."
            value={filterText}
            onChange={e => setFilterText(e.target.value)}
          />
        </div>
      </div>

      <div className="mb-4">
        <div className="d-flex flex-wrap">
          {tags.map(tag => (
            <button
              key={tag}
              className={`btn btn-sm me-2 mb-2 ${selectedTags.has(tag) ? 'btn-warning active' : 'btn-outline-warning'}`}
              onClick={() => toggleTag(tag)}
            >
              {tag}
            </button>
          ))}
        </div>
      </div>

      {filteredRecipes.length > 0 ? (
        <div className="row">
          {filteredRecipes.map(recipe => (
            <div
              key={recipe.id}
              className="col-md-4 mb-4"
              onClick={() => navigate(`/recipe/${recipe.id}`)}
              style={{ cursor: 'pointer' }}
            >
              <div className="card h-100">
                <div className="position-relative">
                  <img
                              src='/assets/recipe.jpeg'
                              className="card-img-top"
                              alt={recipe.name}
                              style={{ objectFit: 'cover', height: '200px' }}
                              
                  />
                  <div
                    className="position-absolute bottom-0 start-0 w-100 text-white p-2"
                    style={{
                      background: 'rgba(0, 0, 0, 0.6)',
                      textAlign: 'center',
                    }}
                  >
                    <h5 className="m-0">{recipe.name}</h5>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <p className="text-center text-muted">No recipes found. Try adjusting your filter.</p>
      )}
    </div>
  );
};

export default RecipeCatalog;

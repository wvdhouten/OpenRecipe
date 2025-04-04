import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Recipe } from '../types/recipe';
import RecipeService from '../services/recipe-service';

const recipeService = new RecipeService();

const RecipeCatalog: React.FC = () => {
  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [filterText, setFilterText] = useState<string>('');
  const [tags, setTags] = useState<string[]>([]);
  const [selectedTags, setSelectedTags] = useState<Set<string>>(new Set());

  useEffect(() => {
    refreshRecipes(false);
  }, []);

  const refreshRecipes = async (hard: boolean) => {
    if (hard) await recipeService.refresh();

    let recipes = await recipeService.getAllRecipes();
    if (!recipes) {
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
    const matchesText = recipe.name.toLowerCase().includes(filterText.toLowerCase());
    const matchesTags =
      selectedTags.size === 0 || Array.from(selectedTags).every(tag => recipe.tags.includes(tag));
    return matchesText && matchesTags;
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
        <div className="input-group w-50">
          <input
            type="text"
            className="form-control"
            placeholder="Filter by name..."
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
              className={`btn btn-sm me-2 mb-2 ${selectedTags.has(tag) ? 'btn-warning' : 'btn-outline-warning'}`}
              onClick={() => toggleTag(tag)}
            >
              {tag}
            </button>
          ))}
        </div>
      </div>

      {filteredRecipes.length > 0 ? (
        <ul className="list-group">
          {filteredRecipes.map(recipe => (
            <li key={recipe.id} className="list-group-item d-flex justify-content-between align-items-center">
              <Link to={`/recipe/${recipe.id}`} className="text-decoration-none">
                {recipe.name}
              </Link>
            </li>
          ))}
        </ul>
      ) : (
        <p className="text-center text-muted">No recipes found. Try adjusting your filter.</p>
      )}
    </div>
  );
};

export default RecipeCatalog;

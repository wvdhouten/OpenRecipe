import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import RecipeCatalog from './components/catalog';
import RecipeDetails from './components/recipe';

const App: React.FC = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<Navigate to="/recipes" />} />
                <Route path="/recipes" element={<RecipeCatalog />} />
                <Route path="/recipe/:id" element={<RecipeDetails />} />
            </Routes>
        </Router>
    );
};

export default App;

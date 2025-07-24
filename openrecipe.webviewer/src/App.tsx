import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import RecipeCatalog from './components/catalog';
import RecipeDetails from './components/recipe';
import Editor from './components/editor';
import SettingsModal from './components/settings.tsx'

const App: React.FC = () => {
    return (
        <>
            <Router>
                <Routes>
                    <Route index element={<RecipeCatalog />} />
                    <Route path="recipe/:id" element={<RecipeDetails />} />
                    <Route path="editor">
                        <Route index element={<Editor />} />
                        <Route path=":id" element={<Editor />} />
                    </Route>
                </Routes>
            </Router>
            <SettingsModal />
        </>
    );
};

export default App;

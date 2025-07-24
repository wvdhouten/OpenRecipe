import React, { useState, useEffect } from 'react';

interface Settings {
  GitHubToken: string;
  RepositoryOwner: string;
  RepositoryName: string;
}

function loadSettings(): Settings {
  return {
    GitHubToken: localStorage.getItem('GitHubToken') ?? '',
    RepositoryOwner: localStorage.getItem('RepositoryOwner') ?? '',
    RepositoryName: localStorage.getItem('RepositoryName') ?? '',
  };
}

const SettingsModal: React.FC = () => {
  const [settings, setSettings] = useState<Settings>({
    GitHubToken: '',
    RepositoryOwner: '',
    RepositoryName: ''
  });

  useEffect(() => {
      setSettings(loadSettings());
  });

  const updateSetting = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setSettings({ ...settings, [name]: value });
    localStorage.setItem(name, value);
  };

  return (
    <div id="settings-modal" className="modal fade">
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header bg-warning">
            <h2 className="h5 mb-0 text-dark">Settings</h2>
            <button type="button" className="btn-close btn-close-white" aria-label="Close" data-bs-dismiss="modal"></button>
          </div>
          <div className="modal-body">
            <form>
              <div className="mb-3">
                <label htmlFor="GitHubToken" className="form-label">
                  GitHub Token
                </label>
                <input type="text" className="form-control" id="GitHubToken" name="GitHubToken" value={settings.GitHubToken} onChange={updateSetting} autoComplete="off" placeholder="Enter your GitHub token" />
              </div>
              <div className="mb-3">
                <label htmlFor="RepositoryOwner" className="form-label">
                  Repository Owner
                </label>
                <input type="text" className="form-control" id="RepositoryOwner" name="RepositoryOwner" value={settings.RepositoryOwner} onChange={updateSetting} autoComplete="off" placeholder="e.g. octocat" />
              </div>
              <div className="mb-3">
                <label htmlFor="RepositoryName" className="form-label">
                  Repository Name
                </label>
                <input type="text" className="form-control" id="RepositoryName" name="RepositoryName" value={settings.RepositoryName} onChange={updateSetting} autoComplete="off" placeholder="e.g. hello-world" />
              </div>
            </form>
          </div>
          <div className="modal-footer">
            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">
              Close
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default SettingsModal;
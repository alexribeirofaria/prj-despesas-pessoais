https://medium.com/@ruben.alapont/npm-workspaces-managing-multi-package-projects-28538fe40d1d

📦 Managing Dependencies Across Workspaces
One of the biggest advantages of using NPM Workspaces is how it simplifies dependency management. Dependencies declared in any workspace package are automatically installed and shared across the entire workspace, avoiding duplication and version conflicts.

Installing Dependencies
Let’s say the core package needs lodash as a dependency. You can install it from the root of your workspace:

npm install lodash --workspace=packages/core

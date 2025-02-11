
# Cat Rescue Portal - Frontend

This is the frontend for the Cat Rescue Portal, built with Angular 18. It provides a user-friendly interface for browsing, adopting, and managing cat profiles.

## Features
- User authentication (login/signup)
- Browse cats with filters (name, breed, age)
- View cat details and submit adoption applications
- Responsive design with Tailwind CSS

## Project Structure
The project follows Angular's recommended LIFT principles 
:

```plaintext
├── angular.json           # Angular workspace configuration
├── package.json          # Project dependencies and scripts
├── src                   # Application source files
│   ├── app              # Main application code
│   │   ├── components   # Reusable UI components
│   │   │   ├── auth     # Authentication components
│   │   │   ├── cat-detail # Cat detail views
│   │   │   └── cat-list  # Cat listing components
│   │   ├── environments # Environment configurations
│   │   ├── guards       # Route protection
│   │   ├── models       # Data models
│   │   └── services     # Business logic services
│   ├── assets           # Static assets
│   ├── global_styles.css # Global CSS styles
│   └── index.html       # Main HTML file
└── tailwind.config.js    # Tailwind CSS configuration
```

## Development Notes
- The project uses feature-based architecture 
, organizing related functionality into distinct modules
- Components are structured to promote reusability and maintainability
- Services handle business logic and API communication
- Guards implement route protection for authenticated features
## Setup
1. Install dependencies:
   ```bash
   npm install
   ```

2. Start the development server:
   ```bash
   ng server
   ```

3. Open your browser and navigate to `http://localhost:4200`.

## Configuration
Update the `environment.ts` file with your backend API URL:
```ts
export const environment = {
  production: false,
  baseUrl: 'http://localhost:5197/api', // Replace with your backend URL
};
```

## Routes
- `/login` - User login
- `/signup` - User registration
- `/cats` - List of available cats
- `/cats/:id` - Cat details and adoption form

## Dependencies
- Angular 18
- Tailwind CSS
- RxJS
- Leaflet (for map view)

## Scripts
- `npm start` - Start the development server
- `npm build` - Build the project for production
- `npm dev` - Alias for `npm start`
```


This README is concise and provides the essential information needed to get started with the frontend. Let me know if you need further adjustments!



import { Routes } from '@angular/router';
import { CatListComponent } from './components/cat-list/cat-list.component';
import { CatDetailComponent } from './components/cat-detail/cat-detail.component';
import { LoginComponent } from './components/auth/login.component';
import { SignupComponent } from './components/auth/signup.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/cats', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'cats', component: CatListComponent },
  { 
    path: 'cats/:id', 
    component: CatDetailComponent,
    canActivate: [authGuard]
  }
];
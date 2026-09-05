import { Routes } from '@angular/router';
import { authGuard, authenticatedGuard, guestGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    canActivate: [guestGuard],
    loadComponent: () => import('./pages/login/login').then((m) => m.LoginComponent)
  },
  {
    path: 'change-password',
    canActivate: [authenticatedGuard],
    loadComponent: () => import('./pages/change-password/change-password').then((m) => m.ChangePasswordComponent)
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./pages/shell/shell').then((m) => m.ShellComponent),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'accueil' },
      { path: 'accueil', loadComponent: () => import('./pages/accueil/accueil').then((m) => m.AccueilComponent) },
      { path: 'cours', loadComponent: () => import('./pages/cours/cours').then((m) => m.CoursComponent) },
      { path: 'salle', loadComponent: () => import('./pages/salle/salle').then((m) => m.SalleComponent) }
    ]
  },
  { path: '**', redirectTo: '' }
];

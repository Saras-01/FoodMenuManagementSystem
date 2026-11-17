import { Routes } from '@angular/router';

import { Dashboard } from './components/dashboard/dashboard';
import { AdminDashboard } from './components/admin-dashboard/admin-dashboard';
import { FoodMenu } from './components/food-menu/food-menu';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    component: Dashboard
  },
  {
    path: 'admin',
    component: AdminDashboard
  },
  {
    path: 'menu',
    component: FoodMenu
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];
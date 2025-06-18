import { Routes } from '@angular/router';
import { AddUserComponent } from './components/add-user/add-user';
import { DashboardComponent } from './components/dashboard/dashboard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'add-user', component: AddUserComponent },
  { path: 'dashboard', component: DashboardComponent }
];
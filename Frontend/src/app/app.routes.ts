import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Rent } from './pages/renting/renting';
import { Transactions } from './pages/transactions/transactions';
import { Signup } from './pages/signup/signup';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'signup', component: Signup },
  { path: 'rent', component: Rent },
  { path: 'transactions', component: Transactions },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];
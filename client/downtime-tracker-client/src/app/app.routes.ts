import { Routes } from '@angular/router';
import { Login } from './features/login/login';
import { DowntimeSelection } from './features/downtime-selection/downtime-selection';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: Login },
  {
    path: 'downtime-selection',
    component: DowntimeSelection,
    canActivate: [authGuard]
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];
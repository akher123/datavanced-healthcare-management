import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  //{
  //  path: 'login',
  //  loadChildren: () =>
  //    import('./auth/auth.module').then(m => m.AuthModule)
  //},
  //{
  //  path: 'dashboard',
  //  canActivate: [authGuard],
  //  loadComponent: () =>
  //    import('./features/dashboard/dashboard.component')
  //      .then(c => c.DashboardComponent)
  //},
  //{
  //  path: '**',
  //  redirectTo: 'login'
  //}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

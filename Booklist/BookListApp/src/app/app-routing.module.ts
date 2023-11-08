import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';

const routes: Routes = [
  {
    path:'', 
    loadChildren: ()=> import('./auth/auth.module').then((m)=>m.AuthModule)
  },
  {
    path:'auth',
    loadChildren: ()=> import('./auth/auth.module').then((m)=>m.AuthModule),
  },
  {
    path:'main-page',
    loadChildren: ()=>import('./main-page/main-page.module').then((m)=>m.MainPageModule),
    canActivate:[AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }

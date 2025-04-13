import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { BasketComponent } from './components/basket/basket.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { AddComponentComponent } from './components/add-component/add-component.component';
import { EditComponentComponent } from './components/edit-component/edit-component.component';
import { ManageComponentsComponent } from './components/manage-components/manage-components.component';
import { AuthGuard } from './guards/auth.guard';
import { AdminGuard } from './guards/admin.guard';
import { PurchaseHistoryComponent } from './components/purchase-history/purchase-history.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'basket', component: BasketComponent, canActivate: [AuthGuard] },
  { path: 'purchase-history', component: PurchaseHistoryComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'admin',
    children: [
      { path: 'add-component', component: AddComponentComponent, canActivate: [AdminGuard] }
    ]
  },
  {
    path: 'admin',
    children: [
      { path: 'edit/:id', component: EditComponentComponent, canActivate: [AdminGuard] },
    ]
  },
  {
    path: 'admin',
    children: [
      { path: 'manage', component: ManageComponentsComponent, canActivate: [AdminGuard] },
    ]
  },
  { path: '**', redirectTo: '' }
];

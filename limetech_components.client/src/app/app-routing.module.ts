import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BasketComponent } from './components/basket/basket.component';
import { HomeComponent } from './components/home/home.component';
import { AddComponentComponent } from './components/add-component/add-component.component';
import { EditComponentComponent } from './components/edit-component/edit-component.component';
import { ManageComponentsComponent } from './components/manage-components/manage-components.component';


const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'basket', component: BasketComponent },
  { path: 'admin/add-component', component: AddComponentComponent },
  { path: 'admin/edit-component/:id', component: EditComponentComponent },
  { path: 'admin/manage-components', component: ManageComponentsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

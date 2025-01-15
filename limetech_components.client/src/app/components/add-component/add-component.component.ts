import { Component } from '@angular/core';
import { ComponentService } from '../../services/component.service';
import { ComponentDTO } from '../../models/component.dto';
import { PartStatus } from '../../models/status.dto';

@Component({
  selector: 'app-add-component',
  templateUrl: './add-component.component.html',
  styleUrl: './add-component.component.css'
})
export class AddComponentComponent {
  newComponent: ComponentDTO = {
    name: null,
    typeOfProduct: null,
    imageUrl: null,
    price: 0,
    purchasedCount: 0,
    productionYear: new Date().getFullYear(),
    powerUsage: 0,
    status: PartStatus.Available,
    stockCount: 0,
    isPublic: false,
  };

  constructor(private componentService: ComponentService) { }

  addComponenet() {
    this.componentService.addComponent(this.newComponent).subscribe(
      (response) => {
        console.log('Componenet added:', response);
        alert('Component successfully added!');
      },
      (error) => {
        console.error('Error adding componenet:', error);
        alert('Failed to add componenet.');
      }
    );
  }
}

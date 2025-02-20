import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ComponentService } from '../../services/component.service';
import { ComponentDTO } from '../../models/component.dto';
import { PartStatus } from '../../models/status.dto';

@Component({
  selector: 'app-add-component',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-component.component.html',
  styleUrl: './add-component.component.css'
})
export class AddComponentComponent {
  addComponentForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private componentService: ComponentService
  ) {
    this.addComponentForm = this.fb.group({
      name: ['', Validators.required],
      producer: ['', Validators.required],
      typeOfProduct: ['', Validators.required],
      imageUrl: [''],
      price: [0, [Validators.required, Validators.min(0)]],
      //purchasedCount: 0,
      productionYear: [new Date().getFullYear(),
      [Validators.required, Validators.min(2000), Validators.max(new Date().getFullYear())]],
      powerUsage: [0, [Validators.required, Validators.min(0)]],
      status: [PartStatus.Available, Validators.required],
      stockCount: [0, [Validators.required, Validators.min(0)]],
      isPublic: [false],
    });
  }

  addComponent() {
    if (this.addComponentForm.invalid) {
      alert('Please fill out the required fields correctly.');
      return;
    }

    const newComponent: ComponentDTO = this.addComponentForm.value;

    this.componentService.addComponent(newComponent).subscribe({
      next: (response) => {
        console.log('Component added:', response);
        alert('Component successfully added!');
        this.addComponentForm.reset({
          name: '',
          producer: '',
          typeOfProduct: '',
          imageUrl: '',
          price: 0,
          productionYear: new Date().getFullYear(),
          powerUsage: 0,
          status: PartStatus.Available,
          stockCount: 0,
          isPublic: false,
        });
      },
      error: (error) => {
        console.error('Error adding component:', error);
        alert('Failed to add component.');
      }
    });
  }
}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ComponentService } from '../../services/component.service';
import { ComponentDTO } from '../../models/component.dto';
import { PartStatus } from '../../models/status.dto';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-component',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit-component.component.html',
  styleUrl: './edit-component.component.css'
})
export class EditComponentComponent implements OnInit {
  componentId!: number;
  editComponentForm!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private componentService: ComponentService
  ) { }


  ngOnInit(): void {
    this.componentId = +this.route.snapshot.paramMap.get('id')!;
    this.loadComponentData();
  }


  loadComponentData() {
    this.componentService.getComponentById(this.componentId).subscribe({
      next: (data) => {
        this.editComponentForm = this.fb.group({
          name: [data.name, Validators.required],
          typeOfProduct: [data.typeOfProduct, Validators.required],
          imageUrl: [data.imageUrl],
          price: [data.price, [Validators.required, Validators.min(0)]],
          productionYear: [data.productionYear, [Validators.required, Validators.min(2000), Validators.max(new Date().getFullYear())]],
          powerUsage: [data.powerUsage, [Validators.required, Validators.min(0)]],
          status: [data.status, Validators.required],
          stockCount: [data.stockCount, [Validators.required, Validators.min(0)]],
          isPublic: [data.isPublic],
        });
      },
      error: (err) => console.error('Error fetching component:', err),
    });
  }

  saveChanges() {
    if (this.editComponentForm.invalid) {
      alert('Please correct the errors before saving.');
      return;
    }

    const updatedComponent: ComponentDTO = this.editComponentForm.value;
    this.componentService.editComponent(this.componentId, updatedComponent).subscribe({
      next: () => {
        alert('Component updated successfully!');
        this.router.navigate(['/admin/manage-components']);
      },
      error: (err) => {
        console.error('Error updating component:', err);
        alert('Failed to update component.');
      }
    });
  }

  cancel() {
    this.router.navigate(['/admin/manage-components']);
  }
}

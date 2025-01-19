import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ComponentService } from '../../services/component.service';
import { ComponentDTO } from '../../models/component.dto';

@Component({
  selector: 'app-manage-components',
  templateUrl: './manage-components.component.html',
  styleUrl: './manage-components.component.css'
})
export class ManageComponentsComponent implements OnInit{

  components: ComponentDTO[] = [];

  constructor(
    private componentService: ComponentService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadComponents();
  }

  loadComponents() {
    this.componentService.getComponents({}).subscribe(
      (data) => (this.components = data),
      (error) => console.error('Failed to load components',error)
    );
  }

  // Add a new component
  addComponent(newComponent: ComponentDTO): void {
    this.componentService.addComponent(newComponent).subscribe({
      next: (component) => {
        console.log('Component added:', component);
        this.loadComponents(); // Refresh the list after adding
      },
      error: (error) => console.error('Failed to add component', error),
    });
  }

  //Edit a new component
  editComponent(id: number) {
    this.router.navigate(['/admin/edit-component', id]);
  }

  // Change visibility of a component
  toggleVisibility(id: number, isPublic: boolean): void {
    this.componentService.toggleVisibility(id, isPublic).subscribe({
      next: () => {
        console.log('Visibility updated');
        this.loadComponents(); // Refresh the list after updating
      },
      error: (error) => console.error('Failed to update visibility', error),
    });
  }



}

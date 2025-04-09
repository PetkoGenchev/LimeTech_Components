import { Component, OnInit } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { ComponentService } from '../../services/home.service';
import { ComponentDTO } from '../../models/component.dto';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PartStatus } from '../../models/status.dto';

@Component({
  selector: 'app-manage-components',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
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

  loadComponents(): void {
    this.componentService.getComponents({}).subscribe({
      next: (data) => (this.components = data),
      error: (error) => console.error('Failed to load components', error),
    });
  }

  //Edit a new component
  editComponent(id: number): void {
    this.router.navigate(['/admin/edit-component', id]);
  }

  // Change visibility of a component
  toggleVisibility(component: ComponentDTO): void {
    const updatedVisibility = !component.isPublic;
    this.componentService.toggleVisibility(component.id, updatedVisibility).subscribe({
      next: () => {
        component.isPublic = updatedVisibility; // Update UI without full reload
        console.log('Visibility updated');
      },
      error: (error) => console.error('Failed to update visibility', error),
    });
  }

  getStatusClass(status: PartStatus): string {
    switch (status) {
      case PartStatus.Available:
        return 'badge bg-success';
      case PartStatus.Sold:
        return 'badge bg-danger';
      case PartStatus.WaitingStock:
        return 'badge bg-secondary';
      default:
        return 'badge bg-light';
    }
  }

}

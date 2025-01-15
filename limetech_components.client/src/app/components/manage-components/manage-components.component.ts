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
    private ComponentService: ComponentService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadComponents();
  }

  loadComponents() {
    this.ComponentService.getComponents({}).subscribe(
      (data) => (this.components = data),
      (error) => console.error(error)
    );
  }

  editComponent(id: number) {
    this.router.navigate(['/admin/edit-component', id]);
  }

  changeVisibility(id: number, isPublic: boolean) {
    this.ComponentService.changeVisibility(id, !isPublic).subscribe(() => {
      this.loadComponents();
    });
  }
}

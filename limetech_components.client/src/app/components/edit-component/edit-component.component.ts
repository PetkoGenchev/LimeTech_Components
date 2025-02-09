import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ComponentService } from '../../services/component.service';
import { ComponentDTO } from '../../models/component.dto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './edit-component.component.html',
  styleUrl: './edit-component.component.css'
})
export class EditComponentComponent implements OnInit {
  componentId!: number;
  componentData!: ComponentDTO;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private componentService: ComponentService
  ) { }


  ngOnInit(): void {
    this.componentId = +this.route.snapshot.paramMap.get('id')!;
    this.loadComponentData();
  }


  loadComponentData() {
    this.componentService.getComponentById(this.componentId).subscribe(
      (data) => (this.componentData = data),
      (error) => console.error(error)
    );
  }


  saveChanges() {
    this.componentService.editComponent(this.componentId, this.componentData)
      .subscribe(() => {this.router.navigate(['/admin/manage-components']);
    });
  }

  cancel() {
    this.router.navigate(['/admin/manage-componenets']);
  }
}

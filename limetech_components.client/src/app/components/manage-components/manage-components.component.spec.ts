import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageComponentsComponent } from './manage-components.component';

describe('ManageComponentsComponent', () => {
  let component: ManageComponentsComponent;
  let fixture: ComponentFixture<ManageComponentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ManageComponentsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageComponentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CatService } from '../../services/cat.service';
import { Cat, AdoptionApplication } from '../../models/cat.model';

@Component({
  selector: 'app-cat-detail',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './cat-detail.html'
})
export class CatDetailComponent implements OnInit {
  cat?: Cat;
  adoptionForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private catService: CatService,
    private fb: FormBuilder
  ) {
    this.adoptionForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      streetAddress: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', [Validators.required, Validators.pattern('^[0-9]{5}$')]],
      adoptionReason: ['', [Validators.required, Validators.minLength(50)]]
    });
  }
  // it's used to get the route parameter from the URL
  ngOnInit() {
    const catId = this.route.snapshot.paramMap.get('id');
    if (catId) {
      this.catService.getCatById(catId).subscribe(cat => {
        this.cat = cat;
      });
    }
  }

  submitApplication() {
    if (this.adoptionForm.valid && this.cat) {
      const formValue = this.adoptionForm.value;
      const application: AdoptionApplication = {
        id: Date.now().toString(),
        catId: this.cat.id,
        applicantName: formValue.fullName,
        email: formValue.email,
        phone: formValue.phone,
        address: `${formValue.streetAddress}, ${formValue.city} ${formValue.postalCode}`,
        hasOtherPets: false,
        hasChildren: false,
        reasonForAdoption: formValue.adoptionReason,
        status: 'pending',
        submittedAt: new Date()
      };

      this.catService.submitAdoptionApplication(application).subscribe(success => {
        if (success) {
          alert('Application submitted successfully!');
          this.adoptionForm.reset();
        }
      });
    }
  }
}
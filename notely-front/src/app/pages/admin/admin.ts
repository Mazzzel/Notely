import { SlicePipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { AdminService } from '../../core/services/admin.service';
import { CompteAdminDTO } from '../../core/models/admin.model';
import { CodePage } from '../../core/models/compte.model';

@Component({
  selector: 'app-admin',
  imports: [SlicePipe],
  templateUrl: './admin.html'
})
export class AdminComponent implements OnInit {
  private adminService = inject(AdminService);

  readonly comptes = signal<CompteAdminDTO[]>([]);

  ngOnInit(): void {
    this.load();
  }

  hasPage(compte: CompteAdminDTO, page: CodePage): boolean {
    return compte.pages.includes(page);
  }

  togglePage(compte: CompteAdminDTO, page: CodePage): void {
    const pages = this.hasPage(compte, page)
      ? compte.pages.filter((p) => p !== page)
      : [...compte.pages, page];

    this.adminService.setPages(compte.idCompte, { pages }).subscribe(() => this.load());
  }

  private load(): void {
    this.adminService.getComptes().subscribe((list) => this.comptes.set(list));
  }
}

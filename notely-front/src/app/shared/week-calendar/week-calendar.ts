import { Component, computed, input, model, output } from '@angular/core';
import { EvenementDTO, TypeEvenement } from '../../core/models/evenement.model';

const DAY_START = 6 * 60;
const DAY_END = 23 * 60;
const PIXELS_PER_HOUR = 42;
const PIXELS_PER_MIN = PIXELS_PER_HOUR / 60;
const GRID_HEIGHT = (DAY_END - DAY_START) * PIXELS_PER_MIN;

interface LaidOutEvent {
  source: EvenementDTO;
  top: number;
  height: number;
  leftPct: number;
  widthPct: number;
  background: string;
  color: string;
  badge: string;
  timeRange: string;
  title: string;
}

interface DayColumn {
  date: Date;
  iso: string;
  isToday: boolean;
  dow: string;
  dnum: number;
  events: LaidOutEvent[];
}

interface HourLabel {
  top: number;
  label: string;
}

export function startOfWeek(d: Date): Date {
  const dt = new Date(d);
  const dow = (dt.getDay() + 6) % 7;
  dt.setDate(dt.getDate() - dow);
  dt.setHours(0, 0, 0, 0);
  return dt;
}

export function addDays(d: Date, n: number): Date {
  const r = new Date(d);
  r.setDate(r.getDate() + n);
  return r;
}

function isSameDay(a: Date, b: Date): boolean {
  return a.getFullYear() === b.getFullYear() && a.getMonth() === b.getMonth() && a.getDate() === b.getDate();
}

function toLocalISODate(d: Date): string {
  const y = d.getFullYear();
  const m = String(d.getMonth() + 1).padStart(2, '0');
  const day = String(d.getDate()).padStart(2, '0');
  return `${y}-${m}-${day}`;
}

function toMinutes(hhmm: string | null | undefined): number | null {
  if (!hhmm) return null;
  const [h, m] = hhmm.split(':').map(Number);
  if (Number.isNaN(h) || Number.isNaN(m)) return null;
  return h * 60 + m;
}

function clamp(v: number, min: number, max: number): number {
  return Math.min(Math.max(v, min), max);
}

function contrastColor(hex: string | undefined): string {
  if (!hex) return '#1c2a3a';
  const c = hex.replace('#', '');
  if (c.length !== 6) return '#1c2a3a';
  const r = parseInt(c.slice(0, 2), 16);
  const g = parseInt(c.slice(2, 4), 16);
  const b = parseInt(c.slice(4, 6), 16);
  const lum = (0.299 * r + 0.587 * g + 0.114 * b) / 255;
  return lum > 0.6 ? '#1c2a3a' : '#ffffff';
}

function typeBadge(type: TypeEvenement): string {
  return type === 'cours' ? 'C' : type === 'examen' ? 'E' : 'S';
}

function defaultColorForType(type: TypeEvenement): string {
  return type === 'cours' ? '#3d6ea5' : type === 'examen' ? '#a8503f' : '#7a5ea8';
}

@Component({
  selector: 'app-week-calendar',
  templateUrl: './week-calendar.html',
  styleUrl: './week-calendar.scss'
})
export class WeekCalendarComponent {
  events = input.required<EvenementDTO[]>();
  eventClick = output<EvenementDTO>();

  readonly weekCursor = model(new Date());

  readonly gridHeight = GRID_HEIGHT;

  readonly hourLabels = computed<HourLabel[]>(() => {
    const labels: HourLabel[] = [];
    for (let m = DAY_START; m <= DAY_END; m += 60) {
      labels.push({
        top: (m - DAY_START) * PIXELS_PER_MIN,
        label: `${String(Math.floor(m / 60)).padStart(2, '0')}:00`
      });
    }
    return labels;
  });

  readonly weekLabel = computed(() => {
    const monday = startOfWeek(this.weekCursor());
    const sunday = addDays(monday, 6);
    const first = monday.toLocaleDateString('fr-FR', { day: 'numeric', month: 'short' });
    const last = sunday.toLocaleDateString('fr-FR', { day: 'numeric', month: 'short', year: 'numeric' });
    return `${first} – ${last}`;
  });

  readonly days = computed<DayColumn[]>(() => {
    const monday = startOfWeek(this.weekCursor());
    const today = new Date();
    const evts = this.events();

    return [...Array(7)].map((_, i) => {
      const date = addDays(monday, i);
      const iso = toLocalISODate(date);

      const dayEvents = evts
        .filter((e) => e.date === iso && e.heureDebut && e.heureFin)
        .map((e) => {
          const start = clamp(toMinutes(e.heureDebut) ?? DAY_START, DAY_START, DAY_END);
          const end = clamp(toMinutes(e.heureFin) ?? DAY_START, DAY_START, DAY_END);
          return { source: e, start, end, col: 0, totalCols: 1 };
        })
        .filter((e) => e.end > e.start)
        .sort((a, b) => a.start - b.start || a.end - b.end);

      const columnEnds: number[] = [];
      dayEvents.forEach((evt) => {
        let placed = false;
        for (let c = 0; c < columnEnds.length; c++) {
          if (columnEnds[c] <= evt.start) {
            columnEnds[c] = evt.end;
            evt.col = c;
            placed = true;
            break;
          }
        }
        if (!placed) {
          columnEnds.push(evt.end);
          evt.col = columnEnds.length - 1;
        }
      });
      const totalCols = columnEnds.length || 1;

      const laidOut: LaidOutEvent[] = dayEvents.map((evt) => {
        const widthPct = 100 / totalCols;
        return {
          source: evt.source,
          top: (evt.start - DAY_START) * PIXELS_PER_MIN,
          height: Math.max(20, (evt.end - evt.start) * PIXELS_PER_MIN),
          leftPct: evt.col * widthPct,
          widthPct,
          background: evt.source.couleur || defaultColorForType(evt.source.type),
          color: contrastColor(evt.source.couleur),
          badge: typeBadge(evt.source.type),
          timeRange: `${evt.source.heureDebut.slice(0, 5)}–${evt.source.heureFin.slice(0, 5)}`,
          title: evt.source.titre
        };
      });

      return {
        date,
        iso,
        isToday: isSameDay(date, today),
        dow: date.toLocaleDateString('fr-FR', { weekday: 'short' }),
        dnum: date.getDate(),
        events: laidOut
      };
    });
  });

  goToday(): void {
    this.weekCursor.set(new Date());
  }

  goPrev(): void {
    this.weekCursor.set(addDays(this.weekCursor(), -7));
  }

  goNext(): void {
    this.weekCursor.set(addDays(this.weekCursor(), 7));
  }

  onEventClick(evt: EvenementDTO): void {
    this.eventClick.emit(evt);
  }
}

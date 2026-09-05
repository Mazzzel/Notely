import { DecimalPipe } from '@angular/common';
import { Component, computed, input } from '@angular/core';
import { ProgressionPointDTO } from '../../core/models/seance.model';

const WIDTH = 640;
const HEIGHT = 220;
const PAD_LEFT = 44;
const PAD_RIGHT = 16;
const PAD_TOP = 16;
const PAD_BOTTOM = 30;

interface ChartPoint {
  x: number;
  y: number;
  value: number;
  label: string;
  showLabel: boolean;
}

function formatShortDate(iso: string): string {
  const [y, m, d] = iso.split('-');
  return `${d}/${m}`;
}

@Component({
  selector: 'app-progression-chart',
  imports: [DecimalPipe],
  templateUrl: './progression-chart.html',
  styleUrl: './progression-chart.scss'
})
export class ProgressionChartComponent {
  points = input.required<ProgressionPointDTO[]>();

  readonly width = WIDTH;
  readonly height = HEIGHT;

  private validPoints = computed(() =>
    this.points()
      .filter((p): p is ProgressionPointDTO & { poidsMax: number } => p.poidsMax !== null)
      .sort((a, b) => a.date.localeCompare(b.date))
  );

  readonly hasData = computed(() => this.validPoints().length > 0);

  readonly yAxis = computed(() => {
    const values = this.validPoints().map((p) => p.poidsMax);
    if (values.length === 0) return { min: 0, max: 0 };
    const min = Math.min(...values);
    const max = Math.max(...values);
    if (min === max) return { min: Math.max(0, min - 5), max: max + 5 };
    const margin = (max - min) * 0.1;
    return { min: Math.max(0, min - margin), max: max + margin };
  });

  readonly chartPoints = computed<ChartPoint[]>(() => {
    const data = this.validPoints();
    const { min, max } = this.yAxis();
    const plotWidth = WIDTH - PAD_LEFT - PAD_RIGHT;
    const plotHeight = HEIGHT - PAD_TOP - PAD_BOTTOM;
    const maxLabels = 8;
    const step = Math.max(1, Math.ceil(data.length / maxLabels));

    return data.map((p, i) => {
      const x = data.length === 1 ? PAD_LEFT + plotWidth / 2 : PAD_LEFT + (i / (data.length - 1)) * plotWidth;
      const ratio = max === min ? 0.5 : (p.poidsMax - min) / (max - min);
      const y = PAD_TOP + (1 - ratio) * plotHeight;
      return {
        x,
        y,
        value: p.poidsMax,
        label: formatShortDate(p.date),
        showLabel: i % step === 0 || i === data.length - 1
      };
    });
  });

  readonly polylinePoints = computed(() => this.chartPoints().map((p) => `${p.x},${p.y}`).join(' '));

  readonly yTicks = computed(() => {
    const { min, max } = this.yAxis();
    const mid = (min + max) / 2;
    const plotHeight = HEIGHT - PAD_TOP - PAD_BOTTOM;
    return [
      { value: max, y: PAD_TOP },
      { value: mid, y: PAD_TOP + plotHeight / 2 },
      { value: min, y: PAD_TOP + plotHeight }
    ];
  });

  readonly baselineY = PAD_TOP + (HEIGHT - PAD_TOP - PAD_BOTTOM);
  readonly plotLeft = PAD_LEFT;
  readonly plotRight = WIDTH - PAD_RIGHT;
}

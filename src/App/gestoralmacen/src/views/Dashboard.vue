<template>
  <div class="py-4 container-fluid">
    <div class="row">
      <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
        <mini-statistics-card
          :title="{ text: 'Ganancia Semanal', value: `$${report.ganancia_Semanal}` }"
          :detail="getGananciaSemanalAnterior"
          :icon="{
            name: 'ni ni-money-coins',
            color: 'text-white',
            background: iconBackground,
          }"
        />
      </div>
      <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
        <mini-statistics-card
          :title="{ text: 'Ganancia Mensual', value: `$${report.ganancia_Mensual}` }"
          :detail="getGananciaMensualAnterior"
          :icon="{
            name: 'ni ni-money-coins',
            color: 'text-white',
            background: iconBackground,
          }"
        />
      </div>
      <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
        <mini-statistics-card
          :title="{ text: 'Compras Mensuales', value: `${report.entradas_Mensuales}` }"
          :detail="getComprasMensualAnterior"
          :icon="{
            name: 'ni ni-cart',
            color: 'text-white',
            background: iconBackground,
          }"
        />
      </div>
      <div class="col-xl-3 col-sm-6 mb-xl-0">
        <mini-statistics-card
          :title="{ text: 'Ventas Mensuales', value: `${report.salidas_Mensuales}` }"
          :detail="getVentasMensualAnterior"
          :icon="{
            name: 'ni ni-cart',
            color: 'text-white',
            background: iconBackground,
          }"
        />
      </div>
    </div>
    <div class="mt-4 row">
      <div class="mb-4 col-lg-5 mb-lg-0">
        <div class="card z-index-2">
          <div class="p-3 card-body">
            <reports-bar-chart
              id="chart-bar"
              title="Total de ganancias"
              :load="report.finish"
              :color=iconBackground
              :chart="{
                labels: [
                  'Ene',
                  'Feb',
                  'Mar',
                  'Abr',
                  'May',
                  'Jun',
                  'Jul',
                  'Ago',
                  'Sep',
                  'Oct',
                  'Nov',
                  'Dic',
                ],
                datasets: {
                  label: 'Ventas',
                  data: `${ report.total_Ganancias_Mensuales }`,
                },
              }"
              :items="[
                {
                  icon: {
                    color: 'primary',
                    component: faUsers,
                  },
                  label: 'Proveedores',
                  progress: { content: `${report.cantidad_Proveedores}`, percentage: 60 },
                },
                {
                  icon: { color: 'info', component: faHandPointer },
                  label: 'Clientes',
                  progress: { content: `${report.cantidad_Clientes}`, percentage: 90 },
                },
                {
                  icon: { color: 'warning', component: faCreditCard },
                  label: 'Ventas',
                  progress: { content: `${report.ganancia_Anual}`, percentage: 30 },
                },
                {
                  icon: { color: 'danger', component: faScrewdriverWrench },
                  label: 'Productos',
                  progress: { content: `${report.tipos_Productos}`, percentage: 50 },
                },
              ]"
            />
          </div>
        </div>
      </div>
      <div class="col-lg-7">
        <!-- line chart -->
        <div class="card z-index-2">
          <gradient-line-chart
            id="chart-line"
            title="Compras y Ventas Mensuales"
            :load="report.finish"
            description="<i class='fa fa-arrow-up text-success'></i>
      <span class='font-weight-bold'>4% more</span> in 2021"
            :chart="{
              labels: [
                'Ene',
                'Feb',
                'Mar',
                'Abr',
                'May',
                'Jun',
                'Jul',
                'Ago',
                'Sep',
                'Oct',
                'Nov',
                'Dic',
              ],
              datasets: [
                {
                  label: 'Total de Ventas',
                  data: `${ report.total_Ventas_Mensuales }`,
                },
                {
                  label: 'Total de Compras',
                  data: `${ report.total_Compras_Mensuales }`,
                },
              ],
            }"
          />
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import MiniStatisticsCard from "@/examples/Cards/MiniStatisticsCard.vue";
import ReportsBarChart from "@/examples/Charts/ReportsBarChart.vue";
import GradientLineChart from "@/examples/Charts/GradientLineChart.vue";
import US from "../assets/img/icons/flags/US.png";
import DE from "../assets/img/icons/flags/DE.png";
import GB from "../assets/img/icons/flags/GB.png";
import BR from "../assets/img/icons/flags/BR.png";
import {
  faHandPointer,
  faUsers,
  faCreditCard,
  faScrewdriverWrench,
} from "@fortawesome/free-solid-svg-icons";

import { API_URL } from '@/config';
import axios from 'axios';

export default {
  name: "dashboard-default",
  data() {
    return {
      iconBackground: "info",
      faCreditCard,
      faScrewdriverWrench,
      faUsers,
      faHandPointer,
      report: {
        finish: false,
        cantidad_Clientes: 0,
        cantidad_Proveedores: 0,
        entradas_Mensuales: 0,
        entradas_Last_Mensuales: 0,
        ganancia_Anual: 0,
        ganancia_Semanal: 0,
        ganancia_Last_Semanal: 0,
        ganancia_Mensual: 0,
        ganancia_Last_Mensual: 0,
        salidas_Mensuales: 0,
        salidas_Last_Mensuales: 0,
        tipos_Productos: 0,
        total_Ganancias_Mensuales: [],
        total_Compras_Mensuales: [],
        total_Ventas_Mensuales: []
      },
      sales: {
        us: {
          country: "United States",
          sales: 2500,
          value: "$230,900",
          bounce: "29.9%",
          flag: US,
        },
        germany: {
          country: "Germany",
          sales: "3.900",
          value: "$440,000",
          bounce: "40.22%",
          flag: DE,
        },
        britain: {
          country: "Great Britain",
          sales: "1.400",
          value: "$190,700",
          bounce: "23.44%",
          flag: GB,
        },
        brasil: {
          country: "Brasil",
          sales: "562",
          value: "$143,960",
          bounce: "32.14%",
          flag: BR,
        },
      },
    };
  },
  components: {
    MiniStatisticsCard,
    ReportsBarChart,
    GradientLineChart,
  },
  mounted() {
    this.GetReports();
  },
  computed: {
    getGananciaSemanalAnterior() {
      let semana_actual = this.report.ganancia_Semanal;
      let semana_anterior = this.report.ganancia_Last_Semanal;
      
      let value = semana_anterior != 0 ? (semana_actual * 100) / semana_anterior : semana_actual;
      let color = semana_actual >= semana_anterior ? "success" : "danger";
      
      let sign = "";
      if (semana_actual < semana_anterior) sign = "-";
      if (semana_actual > semana_anterior) sign = "+";
      
      return `<span class='text-${color} text-sm font-weight-bolder'>${sign}${value}%</span> que la última semana`;
    },

    getGananciaMensualAnterior() {
      let mes_actual = this.report.ganancia_Mensual;
      let mes_anterior = this.report.ganancia_Last_Mensual;
      
      let value = mes_anterior != 0 ? (mes_actual * 100) / mes_anterior : mes_actual;
      let color = mes_actual >= mes_anterior ? "success" : "danger";
      
      let sign = "";
      if (mes_actual < mes_anterior) sign = "-";
      if (mes_actual > mes_anterior) sign = "+";

      return `<span class='text-${color} text-sm font-weight-bolder'>${sign}${value}%</span> que el último mes`;
    },
    
    getComprasMensualAnterior() {
      let actual = this.report.entradas_Mensuales;
      let anterior = this.report.entradas_Last_Mensuales;
      
      let value = anterior != 0 ? (actual * 100) / anterior : actual;
      let color = actual >= anterior ? "success" : "danger";
      
      let sign = "";
      if (actual < anterior) sign = "-";
      if (actual > anterior) sign = "+";

      return `<span class='text-${color} text-sm font-weight-bolder'>${sign}${value}%</span> que el último mes`;
    },
    
    getVentasMensualAnterior() {
      let actual = this.report.salidas_Mensuales;
      let anterior = this.report.salidas_Last_Mensuales;
      
      let value = anterior != 0 ? (actual * 100) / anterior : actual;
      let color = actual >= anterior ? "success" : "danger";
      
      let sign = "";
      if (actual < anterior) sign = "-";
      if (actual > anterior) sign = "+";

      return `<span class='text-${color} text-sm font-weight-bolder'>${sign}${value}%</span> que el último mes`;
    },
  },
  methods: {
    async GetReports() {
      axios.get(`${API_URL}/report`)
        .then(res => {

          this.report.cantidad_Clientes = res.data.value.cantidad_Clientes;
          this.report.cantidad_Proveedores = res.data.value.cantidad_Proveedores;
          this.report.entradas_Mensuales = res.data.value.entradas_Mensuales;
          this.report.entradas_Last_Mensuales = res.data.value.entradas_Last_Mensuales;
          this.report.ganancia_Anual = res.data.value.ganancia_Anual;
          this.report.ganancia_Semanal = res.data.value.ganancia_Semanal;
          this.report.ganancia_Last_Semanal = res.data.value.ganancia_Last_Semanal;
          this.report.ganancia_Mensual = res.data.value.ganancia_Mensual;
          this.report.ganancia_Last_Mensual = res.data.value.ganancia_Last_Mensual;
          this.report.salidas_Mensuales = res.data.value.salidas_Mensuales;
          this.report.salidas_Last_Mensuales = res.data.value.salidas_Last_Mensuales;
          this.report.tipos_Productos = res.data.value.tipos_Productos;
          this.report.total_Ganancias_Mensuales = res.data.value.total_Ganancias_Mensuales;
          this.report.total_Compras_Mensuales = res.data.value.total_Compras_Mensuales;
          this.report.total_Ventas_Mensuales = res.data.value.total_Ventas_Mensuales;

          this.report.finish = true;
        })
        .catch(error => {
          if (error.response && error.response.data) {
            this.error_msg = error.response.data.title;
          } else {
            this.error_msg = error.message;
          }
      });
    }
  }
};
</script>

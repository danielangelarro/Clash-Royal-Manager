<template>
  <Transition name="modal">
    <div v-if="show" class="modal-mask">
      <div class="modal-container">
        <div class="modal-header">
          <slot name="header">default header</slot>
          <button
          class="btn btn-danger"
          @click="emitSelectedItems"
            >X</button>
          </div>
          
        <div class="modal-body container-fluid">
          <div class="row">
            <div class="col-xl-4 col-sm-6">
              <div class="">
                <mini-statistics-card
                  :title="{ 
                    text: 'Estado', 
                    value: `${disponibleCasillero}`
                  }"
                  :icon="{
                    name: 'ni ni-money-coins',
                    color: 'text-white',
                    background: `${getStyleDisponible}`,
                  }"
                />
              </div>
              <div class="">
                <mini-statistics-card
                  :title="{ 
                    text: 'Capacidad en Volumen', 
                    value: `${capacidad_Volumen} / ${volumen_Maximo}` 
                  }"
                  :icon="{
                    name: 'fa fa-cube',
                    color: 'text-white',
                    background: 'success',
                  }"
                />
              </div>
              <div class="">
                <mini-statistics-card
                  :title="{ 
                    text: 'Capacidad en Peso', 
                    value: `${capacidad_Peso} / ${casillero.peso_Maximo}`
                  }"
                  :icon="{
                    name: 'ni ni-money-coins',
                    color: 'text-white',
                    background: 'success',
                  }"
                />
              </div>
            </div>
            <div class="col-xl-8 col-sm-6">
              <inventory-casillero-table
                :products="productos"
                @emit-product-confirm="productConfirm"
              >
              </inventory-casillero-table>
            </div>
          </div>
        </div>
      </div>
    </div>
  </Transition>
</template>

<script>
import MiniStatisticsCard from '@/examples/Cards/MiniStatisticsCard.vue';
import InventoryCasilleroTable from './InventoryCasilleroTable.vue';
import { API_URL } from '@/config';
import axios from 'axios';

export default {
  name: 'casillero-view-info',
  props: ['id', 'show'],
  components: {
    MiniStatisticsCard,
    InventoryCasilleroTable
  },
  data() {
    return {
      selectedItems: [],
      casillero: null,
      productos: [],
      capacidad_Peso: 0,
      capacidad_Volumen: 0,
      volumen_Maximo: 0,
      unidad_medida: ''
    }
  },
  computed: {
    disponibleCasillero() {
      for (const product of this.productos) {
        if (!product.confirmar_Guardado) {
          return "Pendiente";
        }
      }

      if (this.capacidad_Volumen === this.volumen_Maximo || this.capacidad_Peso === this.casillero.peso_Maximo) {
        return "Ocupado";
      }

      return "Disponible";
    },

    getStyleDisponible() {
      
      if (this.disponibleCasillero === "Pendiente") {
        return "warning";
      }
      
      if (this.disponibleCasillero === "Ocupado") {
        return "danger";
      }
      
      return "success";
    }
  },
  watch: {
    show: async function (newShow){

      if (newShow) {
        await this.getCasilleroInfo();
      }
    }
  },
  methods: {
    selectItem(item) {
      const index = this.selectedItems.indexOf(item);
      if (index >= 0) {
        this.selectedItems.splice(index, 1);
      } else {
        this.selectedItems.push(item);
      }
    },

    emitSelectedItems() {
      this.$emit('close');
      this.$emit('selected-items', this.selectedItems);
    },

    async getCasilleroInfo() {
      axios.get(`${API_URL}/casillero/${this.id}`)
        .then(res => {
          this.casillero = res.data.value.casillero;
          this.productos = res.data.value.productos;
          this.capacidad_Peso = res.data.value.capacidad_Peso;
          this.capacidad_Volumen = res.data.value.capacidad_Volumen;
          this.volumen_Maximo = this.casillero.largo * this.casillero.ancho * this.casillero.alto;
          this.unidad_medida = this.casillero.unidad_Dimensiones	;
        })
        .catch(error => {
          if (error.response && error.response.data) {
            this.error_msg = error.response.data.title;
          } else {
            this.error_msg = error.message;
          }
        });
    },

    async productConfirm(iD_Ubicacion) {

      axios.post(`${API_URL}/product/transaction/confirm`, { Ubicacion: iD_Ubicacion })
        .then(res => {
          res;
          this.getCasilleroInfo();
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
}
</script>

<style>

.modal-mask {
    position: fixed;
    z-index: 9998;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    transition: opacity 0.3s ease;
  }
  
  .modal-container {
    width: 70%;
    margin: auto;
    padding: 20px 30px;
    background-color: #fff;
    border-radius: 2px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
    transition: all 0.3s ease;
  }
  
  .modal-header h3 {
    margin-top: 0;
    color: #42b983;
  }
  
  .modal-body {
    margin: 20px 0;
  }
  
  .modal-default-button {
    float: right;
  }
  
  /*
   * The following styles are auto-applied to elements with
   * transition="modal" when their visibility is toggled
   * by Vue.js.
   *
   * You can easily play with the modal transition by editing
   * these styles.
   */
  
  .modal-enter-from {
    opacity: 0;
  }
  
  .modal-leave-to {
    opacity: 0;
  }
  
  .modal-enter-from .modal-container,
  .modal-leave-to .modal-container {
    -webkit-transform: scale(1.1);
    transform: scale(1.1);
  }
</style>

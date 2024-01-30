<template>
  <div class="mb-4 col-xl-12 col-md-6 mb-xl-0">
    <div class="card card-blog card-plain">
      <div class="row">
        <div class="col-6">
          <a class="shadow-xl d-block border-radius-xl">
            <img
              :src="img"
              alt="img-blur-shadow"
              class="shadow img-fluid border-radius-xl"
            />
          </a>
        </div>
        <div v-if="product != null" class="px-1 pb-0 card-body col-6">
          <p class="mb-2 text-sm text-gradient text-dark">{{ product.descripcion }}</p>
          <a href="javascript:;">
            <h5>{{ product.nombre }}</h5>
          </a>
          <div class="table-responsive">
              <table class="table table-borderless">
                  <tbody>
                      <tr><th>ID:</th><td>{{ product.iD_Producto }}</td></tr>
                      <tr><th>Precio Total:</th><td>${{ product.precio_Total }}</td></tr>
                      <tr><th>Precio Unitario:</th><td>${{ product.precio_Unitario }}</td></tr>
                      <tr><th>Tipo:</th><td>{{ product.tipo }}</td></tr>
                      <tr><th>Dimensiones:</th><td>{{ product.alto }} x {{ product.ancho }} x {{ product.largo }} {{ product.unidad_Dimensiones }}</td></tr>
                      <tr><th>Peso:</th><td>{{ product.peso }}</td></tr>
                      <tr><th>En Almacén:</th><td>{{ product.enAlmacen }}</td></tr>
                      <!-- <tr><th>Ubicaciones:</th><td>{{ product.ubicaciones }}</td></tr>
                      <tr><th>Mermas:</th><td>{{ product.mermas }}</td></tr>
                      <tr><th>Entradas:</th><td>{{ product.entradas }}</td></tr>
                      <tr><th>Salidas:</th><td>{{ product.salidas }}</td></tr> -->
                  </tbody>
              </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import img from "@/assets/img/curved-images/curved14.jpg";
import { API_URL } from '@/config';
import axios from 'axios';

export default {
  name: "DetailProductCard",
  props: ['id'],
  data() {
    return {
      img,
      product: null,
      selectedProduct: null,
    };
  },
  mounted() {
    this.getProductById(this.id);
  },
  watch: {
    id(newVal) {
      this.getProductById(newVal);
    }
  },
  methods: {
    async getProductById(productId) {
      axios.get(`${API_URL}/product/${productId}`)
        .then(res => {
          this.product = res.data.value.producto;
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

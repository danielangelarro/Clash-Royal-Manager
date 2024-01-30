<template>
  <navbar btn-background="bg-gradient-primary" />
  <div
    class="pt-5 m-3 page-header align-items-start min-vh-50 pb-11 border-radius-lg"
    :style="{
      backgroundImage:
        'url(' + require('@/assets/img/curved-images/curved6.jpg') + ')',
    }"
  >
    <span class="mask bg-gradient-dark opacity-6"></span>
    <div class="container">
      <div class="row justify-content-center">
        <div class="mx-auto text-center col-lg-5">
          <h1 class="mt-5 mb-2 text-white">Bienvenido!</h1>
          <p class="text-white text-lead">
            Reg&iacute;strese en nuestra aplicaci&oacute;n para comenzar a administrar su almac&eacute;n.
          </p>
        </div>
      </div>
    </div>
  </div>
  <div class="container">
    <div class="row mt-lg-n10 mt-md-n11 mt-n10 justify-content-center">
      <div class="mx-auto col-xl-4 col-lg-5 col-md-7">
        <div class="card z-index-0">
          <div class="pt-4 text-center card-header">
            <h5>Registro</h5>
          </div>
          <div class="card-body">
            <form role="form" @submit.prevent="signUp">
              <div class="mb-3">
                <soft-input
                  :value="name"
                  @input="event => name = event.target.value"
                  id="name"
                  type="text"
                  placeholder="Name"
                  aria-label="Name"
                />
              </div>
              <div class="mb-3">
                <soft-input
                  :value="lastname"
                  @input="event => lastname = event.target.value"
                  id="lastname"
                  type="text"
                  placeholder="Last Name"
                  aria-label="LastName"
                />
              </div>
              <div class="mb-3">
                <soft-input
                  :value="email"
                  @input="event => email = event.target.value"
                  id="email"
                  type="email"
                  placeholder="Email"
                  aria-label="Email"
                />
              </div>
              <div class="mb-3">
                <soft-input
                  :value="password"
                  @input="event => password = event.target.value"
                  id="password"
                  type="password"
                  placeholder="Password"
                  aria-label="Password"
                />
              </div>
              <soft-checkbox
                v-model="flexCheckDefault"
                id="flexCheckDefault"
                name="flexCheckDefault"
                class="font-weight-light"
                checked
              >
                Acepto los
                <a href="javascript:;" class="text-dark font-weight-bolder"
                  >T&eacute;rminos y Condiciones</a
                >
              </soft-checkbox>

              <soft-alert v-if="error_msg != ''" class="font-weight-light" color="danger" dismissible>
                <span class="text-sm">{{ error_msg }}</span>
              </soft-alert>

              <div class="text-center">
                <soft-button
                  color="dark"
                  full-width
                  variant="gradient"
                  class="my-4 mb-2"
                  >Registrar</soft-button
                >
              </div>
              <p class="text-sm mt-3 mb-0">
                Tienes una cuenta?
                <router-link
                  :to="{ name: 'Sign In' }"
                  class="text-dark font-weight-bolder"
                >
                  Loguear
                </router-link>
              </p>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
  <app-footer />
</template>

<script>
import Navbar from "@/examples/PageLayout/Navbar.vue";
import AppFooter from "@/examples/PageLayout/Footer.vue";
import SoftInput from "@/components/SoftInput.vue";
import SoftCheckbox from "@/components/SoftCheckbox.vue";
import SoftButton from "@/components/SoftButton.vue";
import { API_URL } from '@/config';

import { mapMutations } from "vuex";
import axios from 'axios';

export default {
  name: "SignupBasic",
  components: {
    Navbar,
    AppFooter,
    SoftInput,
    SoftCheckbox,
    SoftButton,
  },
  data() {
    return {
      name: '',
      lastname: '',
      email: '',
      password: '',
      flexCheckDefault: false,
      error_msg: ''
    };
  },
  created() {
    this.toggleEveryDisplay();
    this.toggleHideConfig();
  },
  beforeUnmount() {
    this.toggleEveryDisplay();
    this.toggleHideConfig();
  },
  methods: {
    ...mapMutations(["toggleEveryDisplay", "toggleHideConfig"]),

    async signUp() {
      
      try {
        if(!this.flexCheckDefault) {
          this.error_msg = 'Debe aceptar nuestros Términos y Condiciones.';
        }

        const response = await axios.post(`${API_URL}/auth/register`, {
          firstname: this.name,
          lastname: this.lastname,
          email: this.email,
          password: this.password
        });

        if (response.status == 200) {
          localStorage.setItem('user-token', response.data.token);

          this.$router.push('/dashboard');
        }
      } catch (error) {
        if (error.response && error.response.data) {
          this.error_msg = error.response.data.title;
        } else {
          this.error_msg = error.message;
        }
      }
    }
  },
};
</script>

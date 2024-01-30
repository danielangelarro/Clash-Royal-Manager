<template>
  <nav
    class="shadow-none navbar navbar-main navbar-expand-lg border-radius-xl"
    v-bind="$attrs"
    id="navbarBlur"
    data-scroll="true"
  >
    <div class="px-3 py-1 container-fluid">
      <breadcrumbs :currentPage="currentRouteName" :textWhite="textWhite" />
      <div
        class="mt-2 collapse navbar-collapse mt-sm-0 me-md-0 me-sm-4"
        :class="this.$store.state.isRTL ? 'px-0' : 'me-sm-4'"
        id="navbar"
      >
        <ul class="ms-md-auto navbar-nav justify-content-end">
          <li v-if="authenticated" class="nav-item d-flex align-items-center">
            <router-link
              :to="{ name: 'Logout' }"
              class="px-0 nav-link font-weight-bold"
              :class="textWhite ? textWhite : 'text-body'"
            >
              <i
                class="fa fa-sign-out"
                :class="this.$store.state.isRTL ? 'ms-sm-2' : 'me-sm-1'"
              ></i>
              <span class="d-sm-inline d-none">Logout </span>
            </router-link>
          </li>
          <li v-else class="nav-item d-flex align-items-center">
            <router-link
              :to="{ name: 'Sign In' }"
              class="px-0 nav-link font-weight-bold"
              :class="textWhite ? textWhite : 'text-body'"
            >
              <i
                class="fa fa-user"
                :class="this.$store.state.isRTL ? 'ms-sm-2' : 'me-sm-1'"
              ></i>
              <span class="d-sm-inline d-none">Sign In </span>
            </router-link>
          </li>
          <li class="nav-item d-xl-none ps-3 d-flex align-items-center">
            <a
              href="#"
              @click="toggleSidebar"
              class="p-0 nav-link text-body"
              id="iconNavbarSidenav"
            >
              <div class="sidenav-toggler-inner">
                <i class="sidenav-toggler-line"></i>
                <i class="sidenav-toggler-line"></i>
                <i class="sidenav-toggler-line"></i>
              </div>
            </a>
          </li>
          <li class="px-3 nav-item d-flex align-items-center">
            <a
              class="p-0 nav-link"
              @click="toggleConfigurator"
              :class="textWhite ? textWhite : 'text-body'"
            >
              <i class="cursor-pointer fa fa-cog fixed-plugin-button-nav"></i>
            </a>
          </li>
          <li
            class="nav-item dropdown d-flex align-items-center"
            :class="this.$store.state.isRTL ? 'ps-2' : 'pe-2'"
          >
            <a
              href="#"
              class="p-0 nav-link"
              :class="[
                textWhite ? textWhite : 'text-body',
                showMenu ? 'show' : '',
              ]"
              id="dropdownMenuButton"
              data-bs-toggle="dropdown"
              aria-expanded="false"
              @click="showMenu = !showMenu"
            >
              <i class="cursor-pointer fa fa-bell"></i>
            </a>
            <ul
              class="px-2 py-3 dropdown-menu dropdown-menu-end me-sm-n4"
              :class="showMenu ? 'show' : ''"
              aria-labelledby="dropdownMenuButton"
            >

              <li class="mb-2" v-for="notificacion in notificaciones" :key="notificacion['iD_Notificacion']">
                <a class="dropdown-item border-radius-md" href="javascript:;">
                  <div class="py-1 d-flex">
                    <div class="my-auto">
                      <img
                        src="../../assets/img/small-logos/logo-spotify.svg"
                        class="avatar avatar-sm bg-gradient-dark me-3"
                        alt="logo spotify"
                      />
                    </div>
                    <div class="d-flex flex-column justify-content-center">
                      <h6 class="mb-1 text-sm font-weight-normal">
                        <span class="font-weight-bold">{{ notificacion['tipo'] }}</span>
                      </h6>
                      <p class="mb-0 text-xs text-secondary">
                        <i class="fa fa-clock me-1"></i>
                        {{ tiempoTranscurrido(notificacion['fecha']) }}
                      </p>
                    </div>
                  </div>
                </a>
              </li>
              <li class="mb-2" v-if="notificaciones.length == 0">
                <a class="dropdown-item border-radius-md mx-auto" href="javascript:;">
                  <div class="py-1 d-flex">
                    <div class="d-flex flex-column justify-content-center">
                      <h6 class="mb-1 text-sm font-weight-normal">
                        <span class="font-weight-bold">Vac&iacute;o</span>
                      </h6>
                    </div>
                  </div>
                </a>
              </li>
            </ul>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>
<script>
import Breadcrumbs from "../Breadcrumbs.vue";
import { mapMutations, mapActions } from "vuex";
import { isAuthenticated } from "@/store/auth";

import { API_URL } from '@/config';
import axios from 'axios';

export default {
  name: "navbar",
  data() {
    return {
      showMenu: false,
      authenticated: false,
      token: localStorage.getItem('user-token'),
      notificaciones: []
    };
  },
  props: ["minNav", "textWhite"],
  created() {
    this.minNav;
  },
  mounted() {
    this.getAllNotifications();
    this.authenticated = isAuthenticated();
  },
  methods: {
    ...mapMutations(["navbarMinimize", "toggleConfigurator"]),
    ...mapActions(["toggleSidebarColor"]),

    toggleSidebar() {
      this.toggleSidebarColor("bg-white");
      this.navbarMinimize();
    },

    async getAllNotifications() {
      axios.post(`${API_URL}/notificacion`, {Token: this.token})
        .then(res => {
          this.notificaciones = res.data.value['notificacions'];
        })
        .catch(error => {
          if (error.response && error.response.data) {
            this.error_msg = error.response.data.title;
          } else {
            this.error_msg = error.message;
          }
        })
      }
    },
  components: {
    Breadcrumbs,
  },
  computed: {
    currentRouteName() {
      return this.$route.name;
    },

    tiempoTranscurrido(fecha) {
      var ahora = new Date();
      var fechaPasada = new Date(fecha);

      var diferenciaEnMilisegundos = ahora - fechaPasada;

      var diferenciaEnSegundos = Math.floor(diferenciaEnMilisegundos / 1000);
      var diferenciaEnMinutos = Math.floor(diferenciaEnSegundos / 60);
      var diferenciaEnHoras = Math.floor(diferenciaEnMinutos / 60);
      var diferenciaEnDias = Math.floor(diferenciaEnHoras / 24);
      var diferenciaEnMeses = Math.floor(diferenciaEnDias / 30.44);
      var diferenciaEnAños = Math.floor(diferenciaEnDias / 365.25);

      if (diferenciaEnSegundos < 60) {
          return "Ahora";
      } else if (diferenciaEnMinutos < 60) {
          return `Han pasado ${diferenciaEnMinutos} minutos desde ${fechaPasada.toDateString()}`;
      } else if (diferenciaEnHoras < 24) {
          return `Han pasado ${diferenciaEnHoras} horas desde ${fechaPasada.toDateString()}`;
      } else if (diferenciaEnDias < 30.44) {
          return `Han pasado ${diferenciaEnDias} días desde ${fechaPasada.toDateString()}`;
      } else if (diferenciaEnMeses < 12) {
          return `Han pasado ${diferenciaEnMeses} meses desde ${fechaPasada.toDateString()}`;
      } else {
          return `Han pasado ${diferenciaEnAños} años, ${diferenciaEnMeses % 12} meses y ${diferenciaEnDias % 30.44} días desde ${fechaPasada.toDateString()}`;
      }
  },

  },
  updated() {
    const navbar = document.getElementById("navbarBlur");
    window.addEventListener("scroll", () => {
      if (window.scrollY > 10 && this.$store.state.isNavFixed) {
        navbar.classList.add("blur");
        navbar.classList.add("position-sticky");
        navbar.classList.add("shadow-blur");
      } else {
        navbar.classList.remove("blur");
        navbar.classList.remove("position-sticky");
        navbar.classList.remove("shadow-blur");
      }
    });
  },
};
</script>

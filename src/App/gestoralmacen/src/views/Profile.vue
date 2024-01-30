<template>
  <div class="container-fluid">
    <div
      class="mt-4 page-header min-height-300 border-radius-xl"
      :style="{
        backgroundImage:
          'url(' + require('@/assets/img/curved-images/curved14.jpg') + ')',
        backgroundPositionY: '50%',
      }"
    >
      <span class="mask bg-gradient-success opacity-6"></span>
    </div>
    <div class="mx-4 overflow-hidden card card-body blur shadow-blur mt-n6">
      <div class="row gx-4">
        <div class="col-auto">
          <div class="avatar avatar-xl position-relative">
            <img
              :src="image_perfil"
              alt="profile_image"
              class="shadow-sm w-100 border-radius-lg"
            />
          </div>
        </div>
        <div class="col-auto my-auto">
          <div class="h-100">
            <h5 class="mb-1">{{ user.firstName }}</h5>
            <p class="mb-0 text-sm font-weight-bold">{{ user.role }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="py-4 container-fluid">
    <div class="mt-3 row">
      <div class="mt-4 col-12 col-md-6 col-xl-6 mt-md-0">
        <profile-info-card
          v-if="!editUser"
          @edit-user="reset"

          title="Información del Perfil"
          :info="{
            firstName: `${user.firstName}`,
            lastName: `${user.lastName}`,
            email: `${user.email}`,
            rol: 'Almacenero',
          }"
          :action="{
            route: 'javascript:;',
            tooltip: 'Edit Profile',
          }"
        />

        <edit-user v-else :user="user" @edit-user-end="this.editUser=false"/>
      </div>
      <div class="mt-4 col-12 col-xl-6 mt-xl-0">
        <div class="card h-100">
          <div class="p-3 pb-0 card-header">
            <h6 class="mb-0">Notificaciones</h6>
          </div>
          <div class="p-3 card-body">
            <ul class="list-group">
              <li
                v-for="notificacion in notificaciones" :key="notificacion['iD_Notificacion']"

                class="px-0 mb-2 border-0 list-group-item d-flex align-items-center"
              >
                <soft-avatar
                  class="me-3"
                  :img="sophie"
                  alt="kal"
                  border-radius="lg"
                  shadow="regular"
                />
                <div
                  class="d-flex align-items-start flex-column justify-content-center"
                >
                  <h6 class="mb-0 text-sm">{{ notificacion['tipo'] }}</h6>
                  <p class="mb-0 text-xs">{{ notificacion['description'] }}</p>
                </div>
                <a
                  class="mb-0 btn btn-link pe-3 ps-0 ms-auto"
                  @click="deleteNotifications(notificacion['iD_Notificacion'])"
                  >Eliminar</a
                >
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ProfileInfoCard from "./components/ProfileInfoCard.vue";
import EditUser from "./components/EditUser.vue";
import SoftAvatar from "@/components/SoftAvatar.vue";
import sophie from "@/assets/img/kal-visuals-square.jpg";
import marie from "@/assets/img/marie.jpg";
import ivana from "@/assets/img/ivana-square.jpg";
import peterson from "@/assets/img/team-4.jpg";
import nick from "@/assets/img/team-3.jpg";
import img1 from "@/assets/img/home-decor-1.jpg";
import img2 from "@/assets/img/home-decor-2.jpg";
import img3 from "@/assets/img/home-decor-3.jpg";
import team1 from "@/assets/img/team-1.jpg";
import team2 from "@/assets/img/team-2.jpg";
import team3 from "@/assets/img/team-3.jpg";
import team4 from "@/assets/img/team-4.jpg";
import {
  faFacebook,
  faTwitter,
  faInstagram,
} from "@fortawesome/free-brands-svg-icons";
import setNavPills from "@/assets/js/nav-pills.js";
import setTooltip from "@/assets/js/tooltip.js";

import { API_URL } from '@/config';
import axios from 'axios';

export default {
  name: "ProfileOverview",
  components: {
    ProfileInfoCard,
    EditUser,
    SoftAvatar
  },
  data() {
    return {
      editUser: false,
      showMenu: false,
      sophie,
      marie,
      ivana,
      peterson,
      nick,
      img1,
      team1,
      team2,
      team3,
      team4,
      img2,
      img3,
      faFacebook,
      faTwitter,
      faInstagram,
      token: localStorage.getItem('user-token'),
      user: {
        email: '',
        firstName: '',
        id: '',
        lastName: '',
        photo: '',
        token: '',
      },
      image_perfil: sophie,
      notificaciones: []
    };
  },

  mounted() {
    this.$store.state.isAbsolute = true;
    setNavPills();
    setTooltip(this.$store.state.bootstrap);

    this.getCurrentUser();
    this.getAllNotifications();
  },
  beforeUnmount() {
    this.$store.state.isAbsolute = false;
  },

  methods: {
    reset() {
      this.getCurrentUser();
      this.editUser=true;
    },

    async getCurrentUser() {
      axios.get(`${API_URL}/auth/current-user/${this.token}`)
        .then(res => {
          this.user = res.data;

          this.getImage();
        })
        .catch(error => {
          if (error.response && error.response.data) {
            this.error_msg = error.response.data.title;
          } else {
            this.error_msg = error.message;
          }
        });

    },
    
    async getImage() {
      let pathToImage = `${API_URL}/fotos/serve-image`;

      axios.post(pathToImage, {path: this.user.photo}, { responseType: 'arraybuffer' })
        .then(res => {
          const blob = new Blob([res.data], { type: 'image/jpg' });
          const urlCreator = window.URL || window.webkitURL;
          this.image_perfil = urlCreator.createObjectURL(blob);

          console.log(blob);
          console.log(this.image_perfil);

        })
        .catch(error => {
          if (error.response && error.response.data) {
            this.error_msg = error.response.data.title;
          } else {
            this.error_msg = error.message;
          }

          console.log(error);
        });
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
        });
    },
    
    async deleteNotifications(id) {
      axios.delete(`${API_URL}/notificacion/${id}`)
        .then(res => {
            res;
        })
        .catch(error => {
          if (error.response && error.response.data) {
            this.error_msg = error.response.data.title;
          } else {
            this.error_msg = error.message;
          }
      });

      this.getAllNotifications();
    }
  }
};
</script>

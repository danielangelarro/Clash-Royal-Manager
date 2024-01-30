<template>
  <div id="app">
    <Sidebar />
    <ContentWrapper />
    <Footer />
    <LogoutModal />
  </div>
</template>

<script>
import Sidebar from '../components/Sidebar.vue'
import ContentWrapper from '../components/ContentWrapper.vue'
import Footer from '../components/Footer.vue'
import LogoutModal from '../components/LogoutModal.vue'

export default {
  name: 'App',
  components: {
    Sidebar,
    ContentWrapper,
    Footer,
    LogoutModal
  }
}
</script>

<style>
/* Global CSS */
</style>

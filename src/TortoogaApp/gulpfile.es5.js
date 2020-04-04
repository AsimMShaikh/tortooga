﻿body {
  /*padding-top: 100px;*/
  padding-bottom: 20px;
  background-color: whitesmoke;
  font-family: 'Titillium Web', sans-serif; }

table {
  width: 100%; }
  table tbody tr > td {
    padding-top: 5px;
    padding-bottom: 5px; }

hr {
  margin-top: 10px;
  margin-bottom: 10px; }

/* Wrapping element */
/* Set some basic padding to keep content from hitting the edges */
.body-content {
  padding-left: 15px;
  padding-right: 15px; }

.row {
  margin-left: 0px;
  margin-right: 0px; }

.container {
  width: auto; }

.top-pad-1 {
  padding-top: 10px; }

/* Set widths on the form inputs since otherwise they're 100% wide */
/*input,
select,
textarea {
    max-width: 280px;
}*/
/* Carousel */
.carousel {
  max-height: 500px;
  margin-left: -15px;
  margin-right: -15px; }

.carousel-caption p {
  font-size: 20px;
  line-height: 1.4; }

/* buttons and links extension to use brackets: [ click me ] */
.btn-bracketed::before {
  display: inline-block;
  content: "[";
  padding-right: 0.5em; }

.btn-bracketed::after {
  display: inline-block;
  content: "]";
  padding-left: 0.5em; }

#information {
  padding-top: 10px; }

#details {
  padding-top: 10px; }

/*START SEARCH RESULT*/
div .search-result {
  padding-top: 15px;
  padding-bottom: 15px;
  border-top: 2px dotted #ccc; }
  div .search-result ul, div .search-result li {
    list-style-type: none;
    margin: 0;
    padding: 0; }

/*END SEARCH RESULT*/
/*Standardize the input size for all*/
/*.form-inline .form-control {
    width: 100%;
}*/
/*Sidebar style*/
.sidebar-panel a {
  color: inherit;
  text-decoration: inherit; }

.sidebar-panel .panel {
  border: 0; }

.sidebar-panel .panel-heading {
  padding: 0; }

.sidebar-panel .list-group {
  margin-bottom: 0; }
  .sidebar-panel .list-group .list-group-item {
    margin-bottom: 0;
    border: 1px solid black; }

/*TODO: Figure how to get bootstrap.ess through grunt so we can use it in our site.less, perhaps assign full widht or half width classes to dashboard block*/
/*Dashboard style*/
.dashboard .dashboard-block {
  padding-bottom: 10px;
  padding-top: 10px;
  height: 350px;
  overflow-y: auto; }

/*--START SIDEBAR--*/
@media screen and (min-width: 768px) {
  .sidebar-nav .input-group {
    width: 100%; }
  .sidebar-nav .cost-slider {
    width: 100%;
    float: left; }
  .sidebar-nav .form-group {
    padding: 5px;
    margin-bottom: 10px; }
  .sidebar-nav .navbar .navbar-collapse {
    padding: 0;
    max-height: none; }
  .sidebar-nav .navbar ul {
    float: none; }
    .sidebar-nav .navbar ul li {
      float: none;
      display: block; }
      .sidebar-nav .navbar ul li a {
        padding-top: 12px;
        padding-bottom: 12px; }
  /*.sidebar-nav .navbar ul :not() {
        display: block;
    }*/ }

/*--END SIDEBAR--*/
/* Hide/rearrange for smaller screens */
@media screen and (max-width: 767px) {
  /* Hide captions */
  .carousel-caption {
    display: none; } }


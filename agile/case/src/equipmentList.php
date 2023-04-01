<DOCTYPE html>
<html>
 <head>
  <title>Equipment List</title>
  <link rel="stylesheet" href="css/template.css">
  <link rel="stylesheet" href="css/equipmentList.css">
 </head>
<body>
<?php require('template.php') ?>

<h1 class="article_heading">Equipment List</h1>

<div id="equipmentList">
  <?php insertEquipmentItems() ?>
</div>

</body>
</html>

<?php
  function insertEquipmentItems() {
    // TODO: get this information from the database
    $equipmentItems = array(
      new EquipmentItem('Router', 'Images/routerRW.jpg', 'Used to connect networks together.'),
      new EquipmentItem('Router', 'Images/routerRW.jpg', 'Used to connect networks together.'),
      new EquipmentItem('Router', 'Images/routerPT.jpg', 'Used to connect networks together.'),
      new EquipmentItem('Router', 'Images/routerPT.jpg', 'Used to connect networks together.'),
    );

    for ($i=0; $i < count($equipmentItems); $i++) {
      echo "<div class='row'>";
      echo '<p>', $equipmentItems[$i]->name, '</p>';
      echo '<p>', $equipmentItems[$i]->description, '</p>';
      echo '</div>';
    }
  }

  class EquipmentItem {
    public $name;
    public $imageLink;
    public $description;

    function __construct($name, $imageLink, $description) {
      $this->name = $name;
      $this->imageLink = $imageLink;
      $this-> description = $description;
    }
  }
?>

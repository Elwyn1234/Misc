<header id="header">
	<h1>NCL Equipment</h1>
</header>
<nav id="navigation_bar">
	<ul>
    <?php injectNavContents(); ?>
  	<!-- <li><a href="security.html">Login</a></li> -->
	</ul>
</nav>

<?php
  function injectNavContents() {
		// get the name of the script that was requested by the client
    $currentPage = $_SERVER['SCRIPT_NAME'];
		// get the file name; strip the path to the file
		$currentPage = preg_replace('/.*\//', '', $currentPage);
		// TODO: hook this up to who is actually logged in
    $userRole = 'user';
    $navContents = 0;
		// TODO: add cases for other roles
    if ($userRole == 'user') {
			$navContents = array(
				new NavLink('Equipment', 'equipmentList.php'),
				new NavLink('My Items', 'returns.php'),
				new NavLink('Log Out', 'logOut.php'),
			);
		}

    for ($i=0; $i < count($navContents); $i++) {
      if ($navContents[$i]->link == $currentPage)
        echo "<li id='current_page'><a href=", $navContents[$i]->link, ">", $navContents[$i]->readableName, "</a></li>";
      else
        echo "<li><a href=", $navContents[$i]->link, ">", $navContents[$i]->readableName, "</a></li>";
    }
  }

	class NavLink {
		public $readableName = "";
		public $link;

		function __construct($readableName, $link) {
			$this->readableName = $readableName;
			$this->link = $link;
		}
	}
?>

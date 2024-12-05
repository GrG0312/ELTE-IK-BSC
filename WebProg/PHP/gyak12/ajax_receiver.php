<?php

if (!empty($_GET)) {
    echo implode(',', $_GET);
}

if (!empty($_POST)) {
    echo json_encode($_POST);
}

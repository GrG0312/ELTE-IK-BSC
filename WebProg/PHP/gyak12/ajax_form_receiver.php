<?php

function handleFormAsHtml() {
    ?>
    <thead>
        <th>Key</th>
        <th>Value</th>
    </thead>
    <tbody>
        <?php foreach($_POST as $key => $value): ?>
            <tr>
                <td><?= $key ?></td>
                <td><?= $value ?></td>
            </tr>
        <?php endforeach; ?>
    </tbody>
    <?php
}

function handleFormAsJSON() {
    echo json_encode($_POST);
}

// handleFormAsHtml();

handleFormAsJSON();
<?php
define("CUPS_ENDLESS", 1000000);

$input = str_split(file_get_contents("example.txt"));

echo "part one: ".play($input, 100)->order;
echo "\npart two: ".play($input, 10000000, true)->mul;

function play($input, $rounds, $endless = false) {
    $labels = prepare($input, $endless);

    $round = 0;
    $cup = $input[0];
    while($round < $rounds) {
        $cup = move($cup, $labels);
        $round++;
    }
 
    $order = order($labels, $endless);

    return (object)array(
        'order' => implode("", $order),
        'mul' => $order[0] * $order[1]
    );
}
function prepare($input, $endless) {
    $labels = new SplFixedArray($endless ? CUPS_ENDLESS +1 : count($input) +1);

    array_walk(
        $input,
        function($label, $idx) use($input, $endless, &$labels) {
            $labels[$label] = [
                array_key_exists($idx-1, $input) ? $input[$idx-1] : ($endless ? CUPS_ENDLESS : end($input)),
                array_key_exists($idx+1, $input) ? $input[$idx+1] : ($endless ? null : reset($input))
            ];
        }
    );

    if($endless) {
        $prev = end($input);
        $next = max($input) +1;
        while($next <= CUPS_ENDLESS) {
            $labels[$prev] = [$labels[$prev][0], $next];
            $labels[$next] = [$prev, null];
            $prev = $next;
            $next++;
        }
        $labels[CUPS_ENDLESS] = [$labels[CUPS_ENDLESS][0], reset($input)];
    }

    return $labels;
}
function move($cup, &$labels) {
    $next = $labels[$cup][1];
    $picked = [];

    while(count($picked) <3) {
        $picked[] = $next;
        $next = $labels[$next][1];
    }

    $target = dest($cup, $labels, $picked);

    $labels[$cup] = [$labels[$cup][0], $labels[$picked[2]][1]];
    $labels[$labels[$picked[2]][1]] = [$cup, $labels[$labels[$picked[2]][1]][1]];
    $labels[$picked[0]] = [$target, $labels[$picked[0]][1]];
    $labels[$picked[2]] = [$labels[$picked[2]][0], $labels[$target][1]];
    $labels[$labels[$target][1]] = [$picked[2], $labels[$labels[$target][1]][1]];
    $labels[$target] = [$labels[$target][0], $picked[0]];

    return $labels[$cup][1];
}
function dest($cup, $labels, $picked) {
    $target = sub($cup, $labels);
    
    while(in_array($target, $picked)) {
        $target = sub($target, $labels);
    }

    return $target;
}
function sub($cup, $labels) {
    return $cup == 1
        ? ($labels->offsetExists(CUPS_ENDLESS)
            ? CUPS_ENDLESS
            : max(array_keys($labels->toArray())))
        : $cup -1;
}
function order($labels, $endless) {
    $sorted = [];
    
    $next = $labels[1][1];
    while($next != 1 && (!$endless || count($sorted) < 2)) {
        $sorted[] = $next;
        $next = $labels[$next][1];
    }

    return $sorted;
}
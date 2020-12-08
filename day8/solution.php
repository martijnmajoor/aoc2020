<?php

$lines = file("input.txt");

echo "part one: ".run($lines)->acc;
echo "\npart two: ".run($lines, true)->acc;

function run($lines, $tryReplace = false) {
    $idx = 0;
    $handled = [];
    $result = new stdClass();
    $result->acc = 0;
    $result->success = false;

    while(true) {
        if(in_array($idx, $handled)) {
            break;
        } elseif($idx == count($lines)) {
            $result->success = true;
            break;
        }

        array_push($handled, $idx);
        
        $line = explode(" ", trim($lines[$idx]));

        switch($line[0]) {
            case "acc":
                $result->acc += $line[1];
                $idx++;
            break;
            case "nop":
                if($tryReplace) {
                    $alt = run(array_replace($lines, array($idx => "jmp ".$line[1])));
                    if($alt->success == true) {
                        return $alt;
                    }
                }

                $idx++;
            break;
            case "jmp":
                if($tryReplace) {
                    $alt = run(array_replace($lines, array($idx => "nop ".$line[1])));
                    if($alt->success == true) {
                        return $alt;
                    }
                }

                $idx += $line[1];
            break;
        }
    }

    return $result;
}
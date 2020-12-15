FILE = 'example.txt'

def nthNumber(target):
    with open(FILE) as f:
        turn = 1
        last = 0
        spoken = {}
        
        for i in [int(x) for x in f.read().replace('\n','').split(',')]:
            spoken[i] = [turn]
            last = i
            turn += 1
        
        while True:
            if turn > target:
                break
            
            if len(spoken[last]) == 1:
                last = 0
            else:
                last = spoken[last][1] - spoken[last][0]
                
                if last not in spoken:
                    spoken[last] = []
            
            spoken[last].append(turn)
            turn += 1

            if len(spoken[last]) == 3:
                spoken[last].pop(0)
    return last

print("Part one: %d" % nthNumber(2020))
print("Part two: %d" % nthNumber(30000000))
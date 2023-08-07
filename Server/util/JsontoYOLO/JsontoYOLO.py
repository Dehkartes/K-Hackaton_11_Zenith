import json
import os

def pascal_voc_to_yolo(x1, y1, x2, y2, image_w, image_h):
    return [((x2 + x1)/(2*image_w)), ((y2 + y1)/(2*image_h)), (x2 - x1)/image_w, (y2 - y1)/image_h]

def createGt():
	# Separate dataset - train, validation, test

	# load Json label data
	jsonFiles = []
	jsonlist = os.listdir('json')
	total = len(jsonlist)


	for i in jsonlist:
		if i != 'yolo':
			jsonFiles.append(json.load(open(f'./json/{i}', 'rt', encoding='UTF-8')))

	for j in range(len(jsonFiles)):
		i = jsonFiles[j]
		fname = jsonlist[j]
		result = open(f'./yolotxt/{fname}.txt', 'w')
		for j in i['annotations']:
			if j['object_class'] == 'garbage_bag':
				k = pascal_voc_to_yolo(j['bbox'][0][0], j['bbox'][0][1], j['bbox'][1][0], j['bbox'][1][1], i['images']['width'], i['images']['height'])
				fstr = f'0 {k[0]} {k[1]} {k[2]} {k[3]}\n'
				result.write(fstr)
		result.close()

createGt()
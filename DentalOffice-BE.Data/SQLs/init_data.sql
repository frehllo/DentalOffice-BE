DO $$
DECLARE MaterialsSectionId INT;
DECLARE LotsSectionId INT;
DECLARE MetalTypeId INT;
DECLARE DentinTypeId INT;
DECLARE ResinTypeId INT;
DECLARE EnamelTypeId INT;
DECLARE DiskTypeId INT;
BEGIN

-- insert into material_types

INSERT INTO main.material_types(name) VALUES
('Metal')
RETURNING id INTO MetalTypeId;

INSERT INTO main.material_types(name) VALUES
('Dentin')
RETURNING id INTO DentinTypeId;

INSERT INTO main.material_types(name) VALUES
('Enamel')
RETURNING id INTO EnamelTypeId;

INSERT INTO main.material_types(name) VALUES
('AcetalResin')
RETURNING id INTO ResinTypeId;

INSERT INTO main.material_types(name) VALUES
('PolycarbonateDisc')
RETURNING id INTO DiskTypeId;

-- insert into sections
INSERT INTO main.sections (title, route, api_string, section_id, "Configuration") VALUES 
('Materiali', '/materials', NULL, NULL, 
'{
    "iconName":"wb_iridescent", 
    "tableHeaderFields": null,
    "formConfiguration": null
}'
)
RETURNING id INTO MaterialsSectionId;

INSERT INTO main.sections (title, route, api_string, section_id, "Configuration") VALUES 
('Lotti', '/lots', NULL, NULL, 
'{
    "iconName":"format_list_numbered", 
    "tableHeaderFields": null,
    "formConfiguration": null
}'
)
RETURNING id INTO LotsSectionId;

INSERT INTO main.sections (title, route, api_string, section_id, "Configuration") VALUES 
('Studi Dentistici', '/studios', '/studios', NULL, 
'{
    "iconName":"store", 
    "tableHeaderFields":
    [
        {
            "field":"name"
        },
        {
            "field":"color"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"input",
            "className":"col-lg-6 col-sm-12",
            "props":{
                "label":"Email",
                "required":true
            }
        },
        {
            "key":"color",
            "type":"color",
            "props":{
                "label":"Colore",
                "required":true
            },
            "className":"col-lg-6 col-sm-12"
        }
    ]
}'
),
('Metallo', '/materials-metal', '/materials/' || CAST(MetalTypeId AS TEXT), MaterialsSectionId, 
'{
    "iconName": null,
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Nome",
                "required":true
            }
        }
    ]
}'
),
('Dentina', '/materials-dentin', '/materials/' || CAST(DentinTypeId AS TEXT), MaterialsSectionId, 
'{
    "iconName": null,
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Nome",
                "required":true
            }
        }
    ]
}'
),
('Smalto', '/materials-enamel', '/materials/' || CAST(EnamelTypeId AS TEXT), MaterialsSectionId, 
'{
    "iconName": null,
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Nome",
                "required":true
            }
        }, 
        {
            "key": "colors",
            "type": "select",
            "props" : {
                "label" : "Colori Dentina",
                "multiple" : true,
                "options" : []
            }
        }
    ]
}'
),
('Resina Acetalica', '/materials-resin', '/materials/' || CAST (ResinTypeId AS TEXT), MaterialsSectionId, 
'{
    "iconName": null, 
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Nome",
                "required":true
            }
        }
    ]
}'
),
('Dischi Policarbonati', '/materials-disks', '/materials/' || CAST(DiskTypeId AS TEXT), MaterialsSectionId, 
'{
    "iconName": null, 
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Nome",
                "required":true
            }
        }
    ]
}'
),
('Colori', '/colors', '/colors', NULL,
'{
    "iconName": "color_lens", 
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Nome",
                "required":true
            }
        }
    ]
}'
),
('Semilavorazioni', '/semiproducts', '/semiproducts', NULL,
'{
    "iconName": "work_outline", 
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Nome",
                "required":true
            }
        }
    ]
}'
),
('Rischi', '/risks', '/risks', NULL,
'{
    "iconName": "error_outlin", 
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Nome",
                "required":true
            }
        }
    ]
}'
),
('Fasi di lavoro', '/stages', '/stages', NULL,
'{
    "iconName": "skip_next",
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Nome",
                "required":true
            }
        }
    ]
}'
),
('Moduli', '/modules', '/modules', NULL,
'{
    "iconName": "insert_drive_file",
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Nome",
                "required":true
            }
        }
    ]
}'
), 
('Metallo', '/lots-metal', '/lots/' || CAST(MetalTypeId AS TEXT), LotsSectionId, 
'{
    "iconName": null,
    "tableHeaderFields":
    [
        {
            "field":"name"
        },
        {
            "field":"code"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"select",
            "className":"col-12",
            "props":{
                "label":"Metallo",
                "required":true,
                "options": []
            }
        },
        {
            "key":"code",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Codice",
                "required":true
            }
        }
    ]
}'
),
('Dentina', '/lots-dentin', '/lots/' || CAST(DentinTypeId AS TEXT), LotsSectionId, 
'{
    "iconName": null,
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"select",
            "className":"col-12",
            "props":{
                "label":"Dentina",
                "required":true,
                "options": []
            }
        },
        {
            "key":"code",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Codice",
                "required":true
            }
        },
        {
            "key":"color",
            "type":"select",
            "className":"col-12",
            "props":{
                "label":"Colore",
                "required":true,
                "options": []
            }
        }
    ]
}'
),
('Smalto', '/lots-enamel', '/lots/' || CAST(EnamelTypeId AS TEXT), LotsSectionId, 
'{
    "iconName": null,
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"select",
            "className":"col-12",
            "props":{
                "label":"Smalto",
                "required":true,
                "options": []
            }
        },
        {
            "key":"code",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Codice",
                "required":true
            }
        }
    ]
}'
),
('Resina Acetalica', '/lots-resin', '/lots/' || CAST(ResinTypeId AS TEXT), LotsSectionId, 
'{
    "iconName": null, 
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"select",
            "className":"col-12",
            "props":{
                "label":"Resina Acetalica",
                "required":true,
                "options": []
            }
        },
        {
            "key":"code",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Codice",
                "required":true
            }
        }
    ]
}'
),
('Dischi Policarbonati', '/lots-disks', '/lots/' || CAST(DiskTypeId AS TEXT), LotsSectionId, 
'{
    "iconName": null, 
    "tableHeaderFields":
    [
        {
            "field":"name"
        }
    ],
    "formConfiguration":
    [
        {
            "key":"name",
            "type":"select",
            "className":"col-12",
            "props":{
                "label":"Dischi Policarbonati",
                "required":true,
                "options": []
            }
        },
        {
            "key":"code",
            "type":"input",
            "className":"col-12",
            "props":{
                "label":"Codice",
                "required":true
            }
        }
    ]
}'
);

END $$


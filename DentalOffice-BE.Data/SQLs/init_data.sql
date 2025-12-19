DO $$
DECLARE MaterialsSectionId INT;
DECLARE LotsSectionId INT;
DECLARE MetalTypeId INT;
DECLARE DentinTypeId INT;
DECLARE ResinTypeId INT;
DECLARE EnamelTypeId INT;
DECLARE DiskTypeId INT;
DECLARE MetalId INT;
DECLARE DentinId INT;
DECLARE EnamelId INT;
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
            "field":"color",
            "cellRenderer":"AGColoredCircle"
        }
    ],
    "formConfiguration":
    [
        {
            "fieldGroup" : [
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
        }
    ]
}'
),
('Metallo', '/materials-' || CAST(MetalTypeId AS TEXT), '/materials-' || CAST(MetalTypeId AS TEXT), MaterialsSectionId, 
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
            "fieldGroup" : [
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
        }
    ]
}'
),
('Dentina', '/materials-' || CAST(DentinTypeId AS TEXT), '/materials-' || CAST(DentinTypeId AS TEXT), MaterialsSectionId, 
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
            "fieldGroup" : [
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
        }
    ]
}'
),
('Smalto', '/materials-' || CAST(EnamelTypeId AS TEXT), '/materials-' || CAST(EnamelTypeId AS TEXT), MaterialsSectionId, 
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
            "fieldGroup" : [
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
                    "key" : "materialProperties",
                    "fieldGroup" : [
                        {
                          "key": "dentinId",
                          "type": "select",
                          "props": {
                            "label": "Dentina",
                            "options": [],
                            "required": true
                          },
                          "className": "col-12"
                        },
                        {
                            "key":"dentinColorsIds",
                            "type":"multi-select",
                            "className":"col-12",
                            "props":{
                                "label":"Colori Dentina",
                                "required":true,
                                "options": []
                            }
                        }
                    ]
                }
            ]
        }
    ]
}'
),
('Resina Acetalica', '/materials-' || CAST (ResinTypeId AS TEXT), '/materials-' || CAST (ResinTypeId AS TEXT), MaterialsSectionId, 
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
            "fieldGroup" : [
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
        }
    ]
}'
),
('Dischi Policarbonati', '/materials-' || CAST(DiskTypeId AS TEXT), '/materials-' || CAST(DiskTypeId AS TEXT), MaterialsSectionId, 
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
            "fieldGroup" : [
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
            "field":"code"
        }
    ],
    "formConfiguration":
    [
        {
            "fieldGroup" : [
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
            "fieldGroup" : [
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
            "field":"description"
        }
    ],
    "formConfiguration":
    [
        {
            "fieldGroup" : [
                {
                    "key":"description",
                    "type":"input",
                    "className":"col-12",
                    "props":{
                        "label":"Nome",
                        "required":true
                    }
                }
            ]
        }
    ]
}'
),
('Fasi di Lavorazione', '/stages', '/stages', NULL,
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
            "fieldGroup" : [
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
        }
    ]
}'
),
('Moduli', '/document_configurations', '/document_configurations', NULL,
'{
    "iconName": "insert_drive_file",
    "tableHeaderFields":
    [
        {
            "field":"name"
        },
        {
            "field":"content"
        },
        {
            "field":"copyCount"
        }
    ],
    "formConfiguration":
    [
        {
            "fieldGroup" : [
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
                    "key":"copyCount",
                    "type":"input",
                    "className":"col-12",
                    "props":{
                        "label":"Numero di Copie",
                        "required":false
                    }
                },
                {
                    "key": "content",
                    "type": "textarea",
                    "props": {
                        "label": "Configurazione",
                        "required": false
                    },
                    "className": "col-12"
                }
            ]
        }
    ]
}'
), 
('Metallo', '/lots-' || CAST(MetalTypeId AS TEXT), '/lots-' || CAST(MetalTypeId AS TEXT), LotsSectionId, 
'{
    "iconName": null,
    "tableHeaderFields":
    [
        {
            "field":"material.name"
        },
        {
            "field":"code"
        }
    ],
    "formConfiguration":
    [
        {
            "fieldGroup" : [
                {
                    "key":"materialId",
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
        }
    ]
}'
),
('Dentina', '/lots-' || CAST(DentinTypeId AS TEXT), '/lots-' || CAST(DentinTypeId AS TEXT), LotsSectionId, 
'{
    "iconName": null,
    "tableHeaderFields":
    [
        {
            "field":"material.name"
        },
        {
            "field":"code"
        },
        {
            "field":"color.code"
        }
    ],
    "formConfiguration":
    [
        {
            "fieldGroup" : [
                {
                    "key":"materialId",
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
                    "key":"colorId",
                    "type":"select",
                    "className":"col-12",
                    "props":{
                        "label":"Colore",
                        "required":true,
                        "options": []
                    }
                }
            ]
        }
    ]
}'
),
('Smalto', '/lots-' || CAST(EnamelTypeId AS TEXT), '/lots-' || CAST(EnamelTypeId AS TEXT), LotsSectionId, 
'{
    "iconName": null,
    "tableHeaderFields":
    [
        {
            "field":"material.name"
        },
        {
            "field":"code"
        }
    ],
    "formConfiguration":
    [
        {
            "fieldGroup" : [
                {
                    "key":"materialId",
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
        }
    ]
}'
),
('Resina Acetalica', '/lots-' || CAST(ResinTypeId AS TEXT), '/lots-' || CAST(ResinTypeId AS TEXT), LotsSectionId, 
'{
    "iconName": null, 
    "tableHeaderFields":
    [
        {
            "field":"material.name"
        },
        {
            "field":"code"
        }
    ],
    "formConfiguration":
    [
        {
            "fieldGroup" : [
                {
                    "key":"materialId",
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
        }
    ]
}'
),
('Dischi Policarbonati', '/lots-' || CAST(DiskTypeId AS TEXT), '/lots-' || CAST(DiskTypeId AS TEXT), LotsSectionId, 
'{
    "iconName": null, 
    "tableHeaderFields":
    [
        {
            "field":"material.name"
        },
        {
            "field":"code"
        }
    ],
    "formConfiguration":
    [
        {
            "fieldGroup" : [
                {
                    "key":"materialId",
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
        }
    ]
}'
);

INSERT INTO main.colors (code) VALUES 
('A1');

INSERT INTO main.materials (name, material_type_id, "MaterialProperties") VALUES 
('Metallo 1', MetalTypeId, null)
RETURNING id INTO MetalId;

INSERT INTO main.materials (name, material_type_id, "MaterialProperties") VALUES 
('Dentina 1', DentinTypeId, null)
RETURNING id INTO DentinId;

INSERT INTO main.materials (name, material_type_id, "MaterialProperties") VALUES 
('Smalto 1', EnamelTypeId, '{ "dentinColorsIds" : [1] }')
RETURNING id INTO EnamelId;

INSERT INTO main.lots (code, material_id, color_id) VALUES 
('12345M', MetalId, null),
('12345D', DentinId, 1),
('12345S', EnamelId, null);

INSERT INTO main.studios (name, color) VALUES 
('Studio 1', '#ababab');

INSERT INTO main.risks (description) VALUES 
('Rischio 1');

INSERT INTO main.stages (name) VALUES 
('Fase 1');

INSERT INTO main.semiproducts (name) VALUES 
('Semilavorazione 1');

END $$


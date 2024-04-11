if (typeof Academia === "undefined") {
    Academia = {};
}

Academia.Aviao = {
    OnSave: async function (executionContext) {
        await this.ExigirConfirmacaoUsuario(executionContext);
    },

    OnLoad: function (executionContext) {
        'use-strict'
        const formContext = executionContext.getFormContext();
        formContext.data.entity.addOnPostSave(this.CriarEmbarqueAoGerarRegistroAviao);
    },

    CriarEmbarqueAoGerarRegistroAviao: async function (executionContext) {
        const formContext = executionContext.getFormContext();

        const stateCode = formContext.getAttribute("statecode")?.getValue();

        if (stateCode !== 0 && formContext.ui.getFormType() !== 1) {
            return;
        }

        const assentos = Array.from({ length: 10 }, (_, i) => i + 1);
        const aviaoId = formContext.data.entity.getId().replace(/[{}]/g, '');

        assentos.forEach(assento => {
            const portao = Math.floor(Math.random() * 3) + 1;

            const data = {
                "academia_aviaoid@odata.bind": `/academia_aviaos(${aviaoId})`,
                "academia_portao": portao,
                "academia_assento": assento
            }

            Xrm.WebApi.createRecord("academia_embarque", data);
        });

        const data = {
            "statecode": 1
        }

        Xrm.WebApi.updateRecord("academia_aviao", aviaoId, data).then(
            function success(result) {
                formContext.data.refresh(false);
                console.log("Avião updated!");
            },
            function (error) {
                console.log(error.message);
            }
        );
    },

    ExigirConfirmacaoUsuario: async function (executionContext) {
        const formContext = executionContext.getFormContext();
        const stateCode = formContext.getAttribute("statecode")?.getValue();

        if (stateCode !== 0 && formContext.ui.getFormType() !== 1) {
            return;
        }

        const eventArgs = executionContext.getEventArgs();

        if (eventArgs.getSaveMode() === 70) {
            eventArgs.preventDefault();
            return;
        }

        eventArgs.disableAsyncTimeout();

        const confirmStrings = {
            text: "Ao salvar, registros de embarques serão criados, deseja continuar?",
            title: "Atenção!",
            confirmButtonLabel: "Sim"
        };
        const confirmOptions = { height: 200, width: 450 };
        return Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
            function (success) {
                if (!success.confirmed) {
                    eventArgs.preventDefault();
                }
            }
        );
    },
}
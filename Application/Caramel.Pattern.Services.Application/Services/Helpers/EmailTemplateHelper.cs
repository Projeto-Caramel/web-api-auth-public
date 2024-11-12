using Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Adopters;
using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Application.Services.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class EmailTemplateHelper
    {
        public static string GetTemporaryPassword(string data)
        {
            return @$"
                        <html>
                        <body>
                            <h2>Olá!</h2>
                            <p>Você está recebendo este e-mail porque sua ONG foi registrada.</p>
                            <p>Sua senha temporária é:</p>
                            <h3 style='background-color: #f0f0f0; padding: 10px; border-radius: 5px;'> {data} </h3>
                            <p>Por favor, redefinir a senha após o seu primeiro acesso.</p>
                            <p>Obrigado!</p>
                            <p>Equipe Caramel!</p>
                        </body>
                        </html>
                    ";
        }

        public static string GetTemporaryConfirmationCode(string data)
        {
            return @$"
                        <html>
                        <body>
                            <h2>Olá!</h2>
                            <p>Você está recebendo este e-mail porque solicitou um código de autenticação.</p>
                            <p>Seu código de autenticação é:</p>
                            <h3 style='background-color: #f0f0f0; padding: 10px; border-radius: 5px;'> {data} </h3>
                            <p>Por favor, utilize este código para concluir o processo de autenticação.</p>
                            <p>Obrigado!</p>
                            <p>Equipe Caramel!</p>
                        </body>
                        </html>
                    ";
        }


        public static string GetAdoptionConfirmationEmail(Partner partner, AdopterInfos adopter, PetInfos petInfo)
        {
            return @$"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            margin: 20px;
                            padding: 0;
                            background-color: #f9f9f9;
                        }}
                        .card {{
                            background-color: #fff;
                            border: 1px solid #ddd;
                            border-radius: 8px;
                            padding: 20px;
                            margin-bottom: 20px;
                            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
                        }}
                        .highlight {{
                            color: #c0392b; /* vermelho */
                            font-weight: bold;
                        }}
                        .heart {{
                            color: #c0392b; /* vermelho */
                            font-size: 24px;
                        }}
                        h1, h2, h3 {{
                            color: #34495e;
                        }}
                    </style>
                </head>
                <body>
                    <h1>Olá, {partner.Name}</h1>
                    <h2>Alguém se interessou em um de seus Pets!</h2>
            
                    <br/>

                    <h2><strong>{adopter.Name}</strong> 💖 <strong>{petInfo.Name}</strong></h2>

                    <div class='card'>
                        <h3>Detalhes do Adotante:</h3>
                        <p><strong>Nome:</strong> <span class='highlight'>{adopter.Name}</span></p>
                        <p><strong>Email:</strong> <span class='highlight'>{adopter.Email}</span></p>
                        <p><strong>Telefone para Contato:</strong> <span class='highlight'>{adopter.AdopterPhone}</span></p>
                        <p><strong>Data de Nascimento:</strong> <span class='highlight'>{adopter.Birthday.ToString("d")}</span></p>
                        <p><strong>Tipo de Residência:</strong> <span class='highlight'>{adopter.ResidencyType.GetDescription()}</span></p>
                        <p><strong>Estilo de Vida:</strong> <span class='highlight'>{adopter.Lifestyle.GetDescription()}</span></p>
                        <p><strong>Experiência com Pets:</strong> <span class='highlight'>{adopter.PetExperience.GetDescription()}</span></p>
                        <p><strong>Tem criança em Casa:</strong> <span class='highlight'>{adopter.HasChildren.GetDescription()}</span></p>
                        <p><strong>Situação Financeira:</strong> <span class='highlight'>{adopter.FinancialSituation.GetDescription()}</span></p>
                        <p><strong>Tempo Livre:</strong> <span class='highlight'>{adopter.FreeTime.GetDescription()}</span></p>
                    </div>

                    <div class='card'>
                        <h3>Detalhes do Pet:</h3>
                        <p><strong>Nome:</strong> <span class='highlight'>{petInfo.Name}</span></p>
                        <p><strong>Descrição:</strong> <span class='highlight'>{petInfo.Description}</span></p>
                        <p><strong>Sexo:</strong> <span class='highlight'>{petInfo.Sex.GetDescription()}</span></p>
                        <p><strong>Tamanho do Pelo:</strong> <span class='highlight'>{petInfo.Coat.GetDescription()}</span></p>
                        <p><strong>Nível de Energia:</strong> <span class='highlight'>{petInfo.EnergyLevel.GetDescription()}</span></p>
                        <p><strong>Porte:</strong> <span class='highlight'>{petInfo.Size.GetDescription()}</span></p>
                        <p><strong>Necessidade de Estímulos Físicos:</strong> <span class='highlight'>{petInfo.StimulusLevel.GetDescription()}</span></p>
                        <p><strong>Temperamento com pessoas:</strong> <span class='highlight'>{petInfo.Temperament.GetDescription()}</span></p>
                        <p><strong>Tolerância com Crianças:</strong> <span class='highlight'>{petInfo.ChildLove.GetDescription()}</span></p>
                        <p><strong>Socialização com Outros Animais:</strong> <span class='highlight'>{petInfo.AnimalsSocialization.GetDescription()}</span></p>
                        <p><strong>Necessidades Especiais:</strong> <span class='highlight'>{petInfo.SpecialNeeds.GetDescription()}</span></p>
                        <p><strong>Queda de Pelos:</strong> <span class='highlight'>{petInfo.Shedding.GetDescription()}</span></p>
                    </div>

                    <p>Por favor, entre em contato com o(a) possível adotante para sanar qualquer dúvida ou para prosseguir com a adoção!</p>
                    <p>Obrigado!</p>
                    <p>Equipe Caramel!</p>
                </body>
                </html>
            ";
        }

    }
}
